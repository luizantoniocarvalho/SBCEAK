using System;
using System.Collections.Generic;
using System.Text;
using SBCEAK.Dominio.Entidades;
using System.Linq.Expressions;
//using Inca.Dominio.Repositorios;

namespace SBCEAK.Dominio.Repositorios
{
    public interface IOperacaoRepositorio : IBaseRepositorio<Operacao>
    {
        IList<Operacao> PesquisarPorNomeOperacao(string nome);       
    }
}

