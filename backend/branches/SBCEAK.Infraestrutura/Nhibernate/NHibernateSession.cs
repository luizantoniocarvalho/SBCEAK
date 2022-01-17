using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using SBCEAK.Infraestrutura.ADO;
using Microsoft.Extensions.Configuration;
using NHibernate;
using System;
using System.Diagnostics;
using System.Reflection;

namespace SBCEAK.Infraestrutura.Nhibernate
{
    public class NHibernateSession
    {

        /*public static ISession OpenSession<T>(IConfiguration configuration, string conexao) where T : class
        {
            ISessionFactory SessionFactory = null;
            var connectionString = configuration.GetConnectionString(conexao);

            if (Debugger.IsAttached)
            {
                SessionFactory = Fluently
               .Configure()
               .Database(OracleManagedDataClientConfiguration.Oracle10.ShowSql().ConnectionString(connectionString))
               .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
               .BuildSessionFactory();
                using (SessionFactory) return SessionFactory.OpenSession();
            }
            else
            {
                SessionFactory = Fluently
               .Configure()
               .Database(OracleManagedDataClientConfiguration.Oracle10.ConnectionString(connectionString))
               .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
               .BuildSessionFactory();
                using (SessionFactory) return SessionFactory.OpenSession();
            }
        }*/

        public static ISession OpenSession<T>(IConfiguration configuration, string conexao) where T : class
        {
            ISessionFactory SessionFactory = null;
            var connectionString = configuration.GetConnectionString(conexao);

            if (Debugger.IsAttached)
            {
                SessionFactory = Fluently
               .Configure()
               .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
               .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
               .BuildSessionFactory();
                using (SessionFactory) return SessionFactory.OpenSession();
            }
            else
            {
                SessionFactory = Fluently
               .Configure()
               .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
               .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
               .BuildSessionFactory();
                using (SessionFactory) return SessionFactory.OpenSession();
            }
        }

        public static ISession OpenSession<T>(IConfiguration configuration, string conexao, Database database) where T : class
        {
            ISessionFactory SessionFactory = null;
            var connectionString = configuration.GetConnectionString(conexao);

            if (Debugger.IsAttached)
            {
                switch (database)
                {
                    case Database.ORACLE:
                        SessionFactory = Fluently
                       .Configure()
                       .Database(OracleManagedDataClientConfiguration.Oracle10.ShowSql().ConnectionString(connectionString))
                       .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
                       .BuildSessionFactory();
                        using (SessionFactory) return SessionFactory.OpenSession();

                    case Database.POSTGRE:
                        SessionFactory = Fluently
                       .Configure()
                       .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
                       .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
                       .BuildSessionFactory();
                        using (SessionFactory) return SessionFactory.OpenSession();

                    default:
                        throw new Exception("Nenhum banco de dados válido foi definido.");
                }

            }
            else
            {
                switch (database)
                {
                    case Database.ORACLE:
                        SessionFactory = Fluently
                       .Configure()
                       .Database(OracleManagedDataClientConfiguration.Oracle10.ConnectionString(connectionString))
                       .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
                       .BuildSessionFactory();
                        using (SessionFactory) return SessionFactory.OpenSession();

                    case Database.POSTGRE:
                        SessionFactory = Fluently
                       .Configure()
                       .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
                       .Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
                       .BuildSessionFactory();
                        using (SessionFactory) return SessionFactory.OpenSession();

                    default:
                        throw new Exception("Nenhum banco de dados válido foi definido.");
                }
            }
        }

        public static ISession OpenSession(IConfiguration configuration, string conexao, Assembly assembly)
        {
            ISessionFactory SessionFactory = null;
            var connectionString = configuration.GetConnectionString(conexao);

            if (Debugger.IsAttached)
            {
                SessionFactory = Fluently
               .Configure()
               .Database(OracleManagedDataClientConfiguration.Oracle10.ShowSql().ConnectionString(connectionString))
               .Mappings(m => m.FluentMappings.AddFromAssembly(assembly))
               .BuildSessionFactory();
                using (SessionFactory) return SessionFactory.OpenSession();
            }
            else
            {
                SessionFactory = Fluently
               .Configure()
               .Database(OracleManagedDataClientConfiguration.Oracle10.ConnectionString(connectionString))
               .Mappings(m => m.FluentMappings.AddFromAssembly(assembly))
               .BuildSessionFactory();
                using (SessionFactory) return SessionFactory.OpenSession();
            }
        }
    }

}
