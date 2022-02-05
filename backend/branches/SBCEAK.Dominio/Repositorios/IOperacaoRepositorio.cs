using System;
using System.Collections.Generic;
using System.Text;
using SBCEAK.Dominio.Entidades;
using SBCEAK.Dominio.Repositorios;
using System.Linq.Expressions;

namespace SBCEAK.Dominio.Repositorios
{
    public interface IOperacaoRepositorio : IBaseRepositorio<Operacao>
    {
        IList<Operacao> PesquisarPorNomeOperacao(string nome);
        
        IList<Operacao> PesquisarPorTodasOperacoes();       
    }
}

