using System;
using System.Collections.Generic;
using System.Data;

namespace SBCEAK.Dominio.Repositorios
{
    public interface IBaseRepositorio<TEntidade> : IDisposable where TEntidade : class
    {
        /// <summary>
        /// Insere ou atualiza os dados da entidade em um repositório
        /// </summary>
        /// <param name="entidade"></param>
        void Salvar(TEntidade entidade);

        /// <summary>
        ///  Cria a entidade em um banco de dados
        /// </summary>
        /// <param name="entidade"></param>
        void Criar(TEntidade entidade);

        void Excluir(TEntidade entidade);
        TEntidade BuscarPorId(object id);
        List<TEntidade> Listar();
        bool Existe(object id);
        IDbConnection AbrirConexao();
        void AbrirTransacao();
        void AbrirTransacao(IsolationLevel isolationLevel);
        void ComitarTransacao();
        void Rollback();
    }
}
