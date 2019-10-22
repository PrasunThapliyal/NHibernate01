
namespace ORM_NHibernate
{
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using System.Reflection;

    public class DBSession
    {
        public ISessionFactory GetSessionFactory()
        {
            // TODO: It should be possible to define this config in some XML file as well
            // Google to find out how

            var cfg = new Configuration();
            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NHibernate01;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                x.Driver<SqlClientDriver>();
                x.Dialect<MsSqlCeDialect>();
            });

            cfg.AddAssembly(Assembly.GetExecutingAssembly());

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
