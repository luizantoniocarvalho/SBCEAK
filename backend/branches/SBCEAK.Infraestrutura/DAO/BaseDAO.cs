using SBCEAK.Dominio.Repositorios;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SBCEAK.Infraestrutura.Nhibernate
{
    /// <summary>
    /// Classe base DAO para conexão com o Banco de Dados, utilizando o NHIbernate
    /// </summary>
    /// <typeparam name="TEntidade"></typeparam>
    public class BaseDAO<TEntidade> : IBaseRepositorio<TEntidade> where TEntidade : class
    {
        /// <summary>
        /// Sessão de conexão com o banco de dados.
        /// </summary>
        protected ISession session;
        ITransaction transaction;
        /// <summary>
        /// Interface de configuração da aplicação, injetada na startup
        /// </summary>
        IConfiguration configuration;
        Database database;
        string conexao;
        Type mapeamento;

        public BaseDAO(ISession session)
        {
            this.session = session;
        }

        public BaseDAO(IConfiguration configuration, string chaveConexao, Type mapeamento, Database? database = null)
        {
            this.configuration = configuration;
            if (database != null) this.database = (Database)database;
            conexao = chaveConexao;
            this.mapeamento = mapeamento;
        }

        public void Salvar(TEntidade entidade)
        {
            session.SaveOrUpdate(entidade);
            session.Flush();
        }

        public async void SalvarAsync(TEntidade entidade)
        {
            await session.SaveOrUpdateAsync(entidade);
            session.Flush();
        }

        public void Criar(TEntidade entidade)
        {
            session.Save(entidade);
            session.Flush();
        }

        public async void CriarAsync(TEntidade entidade)
        {
            await session.SaveAsync(entidade);
            session.Flush();
        }

        public virtual void Excluir(TEntidade entidade)
        {
            session.Delete(entidade);
        }

        public async virtual void ExcluirAsync(TEntidade entidade)
        {
            await session.DeleteAsync(entidade);
        }

        public TEntidade BuscarPorId(object id)
        {
            return session.Get<TEntidade>(id);
        }

        public async Task<TEntidade> BuscarPorIdAsync(object id)
        {
            return await session.GetAsync<TEntidade>(id);
        }

        public List<TEntidade> Listar()
        {
            return session.Query<TEntidade>().ToList();
        }

        public async Task<List<TEntidade>> ListarAsync()
        {
            return await session.Query<TEntidade>().ToListAsync();
        }

        public List<TEntidade> ListarPaginar(int primeiroRegistro, int quantidadeRegistro, Expression<Func<TEntidade, bool>> filtro, out int total)
        {
            total = session.Query<TEntidade>().Where(filtro).Count();
            return session.Query<TEntidade>().Where(filtro).Skip(primeiroRegistro).Take(quantidadeRegistro).ToList();
        }

        public bool Existe(object id)
        {
            var entidade = session.Get<TEntidade>(id);
            return entidade != null;
        }

        public void AbrirTransacao()
        {
            transaction = session.BeginTransaction();
        }

        public void AbrirTransacao(IsolationLevel isolationLevel)
        {
            transaction = session.BeginTransaction(isolationLevel);
        }

        public void ComitarTransacao()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            if (transaction != null)
            {
                if (transaction.IsActive)
                    transaction.Rollback();
                transaction.Dispose();
            }

            if (session != null)
                session.Dispose();
        }

        public IDbConnection AbrirConexao()
        {
            session = NHibernateSession.OpenSession(configuration, conexao, mapeamento.Assembly);
            return session.Connection;
        }

    }
}

