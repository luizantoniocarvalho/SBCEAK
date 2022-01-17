using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SBCEAK.Dominio.Entidades;
using SBCEAK.Dominio.Repositorios;
using NHibernate;
using NHibernate.Util;
using System;
using SBCEAK.Infraestrutura.Nhibernate;
using NHibernate.Transform;

namespace SBCEAK.Infraestrutura.DAO
{
    public class OperacaoDAO : BaseDAO<Dominio.Entidades.Operacao>, IOperacaoRepositorio
    {
        public OperacaoDAO(ISession session) : base(session)
        {
        }
        public IList<Operacao> PesquisarPorNomeOperacao(string nome)
        {
            return session.Query<Operacao>().Where(p => p.ds_Nome_Operacao.ToUpper().Contains(nome.ToUpper())).ToList();
        }          
  
    }
}