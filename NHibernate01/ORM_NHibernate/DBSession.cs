
namespace ORM_NHibernate
{
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using ORM_NHibernate.Infrastructure;
    using System.Reflection;

    public class DBSession
    {
        public ISessionFactory GetSessionFactory()
        {
            // TODO: It should be possible to define this config in some XML file as well
            // Google to find out how

            var cfg = new NHibernate.Cfg.Configuration();
            {
                // This way of configuring works, but I want to try out the xml config file
                //cfg.DataBaseIntegration(x =>
                //{
                //    x.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NHibernate01;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                //    x.Driver<SqlClientDriver>();
                //    x.Dialect<MsSqlCeDialect>();
                //});

                //cfg.AddAssembly(Assembly.GetExecutingAssembly());
            }
            {
                // Use xml for configuration
                // Add hibernate.cfg.xml as Embedded Resource in a folder named Configuration
                // use cfg.Configure(assembly, resourceName) version
                // The other version cfg.Configure(resourceName) always searches for the file in startup project

                var assembly = Assembly.GetExecutingAssembly();
                var assemblyName = assembly.GetName().Name;
                var manifestResourceName = "Configuration.hibernate.cfg.xml";
                cfg.Configure(assembly, $"{assemblyName}.{manifestResourceName}");

                cfg.SetInterceptor(new AuditInterceptor());
            }

            // Note: Troubleshooting
            // At some point here, I was getting error which got resolved due to this:
            // https://stackoverflow.com/questions/35444487/how-to-use-sqlclient-in-asp-net-core
            //            Instead of referencing System.Data and System.Data.SqlClient you need to grab from Nuget:
            //            System.Data.Common and System.Data.SqlClient


            var sefact = cfg.BuildSessionFactory();

            // Later on, use like this:

            //using (var session = sefact.OpenSession())
            //{
            //    using (var tx = session.BeginTransaction())
            //    {
            //        //perform database logic 
            //        tx.Commit();
            //    }
            //    Console.ReadLine();
            //}

            return sefact;
        }
    }
}
