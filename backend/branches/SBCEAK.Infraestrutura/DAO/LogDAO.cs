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
    public class LogDAO : BaseDAO<Dominio.Entidades.LogRegistro>, ILogRepositorio
    {
        public LogDAO(ISession session) : base(session)
        {
        }

        public IList<LogRegistro> PesquisarPorTodasLogs()
        {
            return session.Query<LogRegistro>().ToList();
        }        
    }
}