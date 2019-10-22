
namespace ORM_NHibernate
{
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using System.Reflection;

    public class DBSession
    {
        public ISessionFactory GetSession()
        {
            var cfg = new Configuration();
            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionString = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NHibernate01;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                x.Driver<SqlClientDriver>();
                //x.Dialect<MsSqlCeDialect>();
                x.Dialect<MsSqlCeDialect>();
            });

            cfg.AddAssembly(Assembly.GetExecutingAssembly());

            var sefact = cfg.BuildSessionFactory();

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
