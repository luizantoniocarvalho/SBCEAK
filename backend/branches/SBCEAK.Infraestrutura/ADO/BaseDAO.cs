using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.Odbc;
using SBCEAK.Dominio.Entidades;
using SBCEAK.Dominio.Repositorios;
using System.Data.SqlClient;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace SBCEAK.Infraestrutura.ADO
{
    public struct Tabela
    {
        public string Chave { get; set; }
        public string Nome { get; set; }
    }

    /// <summary>
    /// Clase Base de acesso aos dados utilando ADO.
    /// </summary>
    /// <typeparam name="TEntidade"></typeparam>    
    public abstract class BaseDAO<TEntidade> : IBaseRepositorio<TEntidade> where TEntidade : EntidadeBase
    {
        Database database;

        // objetos ADO.NET
        protected DbConnection connection;
        protected DbDataReader dataReader;
        protected DbCommand command;
        protected DbTransaction transaction;

        // string de conexão e tabela
        protected string conexao = "";
        protected Tabela Tabela;


        public BaseDAO(IConfiguration configuration, string chaveConexao, Database? database = null)
        {
            if (string.IsNullOrWhiteSpace(chaveConexao))
                throw new Exception("a chave de conexão não pode ser vazia");

            conexao = configuration.GetConnectionString(chaveConexao);
            if (string.IsNullOrWhiteSpace(conexao))
                throw new Exception("a chave de conexão informada não exise no arquivo de configuração.");

            if (database == null)
            {
                string bd = configuration.GetSection("Database").Get<string>();
                if (string.IsNullOrWhiteSpace(bd))
                    throw new Exception("Nenhum banco de dados foi informado, adicione o SGBD no campo DatabaseHelper.Database");
                database = (Database)Enum.Parse(typeof(Database), bd.ToUpper());
            }
            else
            {
                this.database = (Database)database;
            }
        }

        public BaseDAO(Database database, string conexao)
        {
            this.database = database;
            this.conexao = conexao;
        }

        void FactoryConnection()
        {
            switch (database)
            {
                case Database.PROGRESS:
                    connection = new OdbcConnection(conexao);
                    command = connection.CreateCommand();
                    break;

                case Database.ORACLE:
                    connection = new OracleConnection(conexao);
                    command = connection.CreateCommand();
                    break;

                default:
                    throw new Exception("Não existe implementação de conexão para o banco de dados informado.");
            }
        }

        protected void AdicionarParametro(string nome, object valor)
        {
            DbParameter parametro = command.CreateParameter();
            parametro.Value = valor != null ? valor : DBNull.Value;
            parametro.ParameterName = nome;
            command.Parameters.Add(parametro);
        }

        protected void AdicionarParametro(string nome, object valor, DbType dbType)
        {
            DbParameter parametro = command.CreateParameter();
            parametro.Value = valor != null ? valor : DBNull.Value;
            parametro.ParameterName = nome;
            parametro.DbType = dbType;
            command.Parameters.Add(parametro);
        }


        public abstract TEntidade GetEntidade(DbDataReader reader);

        protected virtual List<TEntidade> GetEntidades(DbDataReader reader)
        {
            List<TEntidade> entidades = new List<TEntidade>();
            while (reader.Read())
                entidades.Add(GetEntidade(reader));
            return entidades;
        }


        public List<TEntidade> Listar()
        {
            if (database == Database.PROGRESS)
                command.CommandText = $"SELECT * FROM pub.\"{Tabela.Nome}\"";
            else
                command.CommandText = $"SELECT * FROM {Tabela.Nome}";

            return GetEntidades(command.ExecuteReader());
        }

        public virtual TEntidade BuscarPorId(object id)
        {
            if (database == Database.PROGRESS)
                command.CommandText = $"SELECT * FROM pub.\"{Tabela.Nome}\"  WHERE \"{Tabela.Chave}\" = '{id}'";
            else
                command.CommandText = $"SELECT * FROM {Tabela.Nome} WHERE {Tabela.Chave} = {id}";

            var entidades = GetEntidades(command.ExecuteReader());
            if (entidades == null || entidades.Count == 0)
            {
                return null;
            }
            return entidades[0];
        }

        public abstract void Salvar(TEntidade entidade);
        public abstract void Criar(TEntidade entidade);

        public void Excluir(TEntidade entidade)
        {
            var id = entidade.GetType().GetProperty("Id").GetValue(entidade);
            //connection = new SqlConnection(conexao);
            //connection.Open();
            if (database == Database.PROGRESS)
                command.CommandText = $"DELETE FROM pub.\"{Tabela.Nome}\" WHERE {Tabela.Chave} = \"{id.ToString()}\"";
            else
                command.CommandText = $"DELETE FROM {Tabela.Nome} WHERE {Tabela.Chave} = {id.ToString()}";

            int linhasAfetadas = command.ExecuteNonQuery();
        }

        public IDbConnection AbrirConexao()
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                FactoryConnection();
                connection.Open();
                return connection;
            }
            else
                throw new Exception($"Não foi possível abrir conexão com o banco de dados.{database.ToString()} ");
        }

        public void AbrirTransacao()
        {
            if (connection == null)
                FactoryConnection();

            transaction = connection.BeginTransaction();
        }

        public void AbrirTransacao(IsolationLevel isolationLevel)
        {
            if (connection == null)
                FactoryConnection();

            transaction = connection.BeginTransaction(isolationLevel);
        }

        public void ComitarTransacao()
        {
            if (transaction.Connection.State == ConnectionState.Open)
            {
                if (transaction != null) transaction.Commit();
                else throw new Exception("A transação não existe.");
            }
            else
                throw new Exception("A conexão está fechada.");
        }
        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();

            }

            if (command != null)
                command.Dispose();

            if (dataReader != null)
            {
                dataReader.Close();
                dataReader.Dispose();
            }

            if (transaction != null)
                transaction.Dispose();


        }

        public void Rollback()
        {
            if (transaction != null)
                transaction.Rollback();
        }

        public bool Existe(object id)
        {
            var entidade = BuscarPorId(id);
            return entidade != null;
        }

        public Byte[] GetBLOB()
        {
            try
            {
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Byte[] bytes = reader[0] as Byte[];
                    return bytes;
                }
                return new byte[0];
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}