
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
        public NHibernate.Cfg.Configuration GetConfiguration()
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


                //// You can also add Mappings by Code .. Note here that we have not defined any HBM for Teacher class
                //// Also not that we've updated the Student HBM file to set VARCHAR(1000) instead of the default 255
                //var mapper = new NHibernate.Mapping.ByCode.ConventionModelMapper();
                //cfg.AddMapping(mapper.CompileMappingFor(new[] { typeof(BusinessObjects.Teacher) }));

                var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
                schemaExport.SetOutputFile(@"db.sql").Execute(useStdOut: true, execute: true, justDrop: false);

                //// Example Schema Export
                ///
                /*
                 * 
                    create table ActorRole (
                        id INTEGER not null,
                       Actor VARCHAR(255) not null,
                       Role VARCHAR(255) not null,
                       MovieId INTEGER,
                       ActorIndex INTEGER,
                       primary key (id)
                    )

                    create table product (
                        id INTEGER not null,
                       Name VARCHAR(255) not null,
                       description VARCHAR(100),
                       UnitPrice NUMERIC(18,4) not null,
                       primary key (id)
                    )

                    create table Book (
                        Id INTEGER not null,
                       ISBN VARCHAR(255),
                       Author VARCHAR(255),
                       primary key (Id)
                    )

                    create table Movie (
                        Id INTEGER not null,
                       Director VARCHAR(255),
                       primary key (Id)
                    )

                    alter table ActorRole
                        add index (MovieId),
                        add constraint FK_B3337C3E
                        foreign key (MovieId)
                        references Movie (Id)

                    alter table Book
                        add index (Id),
                        add constraint FK_2E5EFA32
                        foreign key (Id)
                        references product (id)

                    alter table Movie
                        add index (Id),
                        add constraint FK_13C98C6D
                        foreign key (Id)
                        references product (id)

                    create table hibernate_unique_key (
                         next_hi INTEGER
                    )

                    insert into hibernate_unique_key values ( 1 )
                 * 
                 * */

                cfg.SetInterceptor(new AuditInterceptor());
            }

            return cfg;
        }

        public ISessionFactory GetSessionFactory(NHibernate.Cfg.Configuration cfg)
        {
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
