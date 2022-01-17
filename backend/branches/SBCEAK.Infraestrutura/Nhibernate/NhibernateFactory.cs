using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;

namespace SBCEAK.Infraestrutura.Nhibernate
{
    public class NhibernateFactory
    {
        private static Dictionary<string, ISessionFactory> sessionFactory = new Dictionary<string, ISessionFactory>();

        public static void CreateFactory<T>(IConfiguration configuration, string conexao, Database database = Database.ORACLE) where T : class
        {

            if (!sessionFactory.ContainsKey(conexao) || sessionFactory[conexao] == null || NhibernateFactory.sessionFactory[conexao].IsClosed)
            {
                var connectionString = configuration.GetConnectionString(conexao);

                switch (database)
                {
                    case Database.ORACLE:
                        sessionFactory.Add(conexao,
                            Fluently
                           .Configure()
                           .Database(OracleManagedDataClientConfiguration.Oracle10.ShowSql().ConnectionString(connectionString))
                           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
                           .BuildSessionFactory());
                        break;

                    case Database.POSTGRE:
                        sessionFactory.Add(conexao,
                        Fluently
                       .Configure()
                       .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
                       .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
                       .BuildSessionFactory());
                        break;

                    default:
                        throw new Exception("Nenhum banco de dados válido foi definido.");
                }
            }
        }

        public static ISession OpenSession<T>(IConfiguration configuration, string conexao, Database database = Database.ORACLE) where T : class
        {
            NhibernateFactory.CreateFactory<T>(configuration, conexao, database);
            return sessionFactory[conexao].OpenSession();
        }
    }
}