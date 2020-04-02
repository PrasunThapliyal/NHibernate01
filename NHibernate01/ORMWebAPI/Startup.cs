using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ORM_NHibernate;
using Newtonsoft.Json;

namespace ORMWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()

                // TODO : Newtonsoft.Json not working in .netcoreapp3.1 .. specifically ReferenceLoopHandling
                //.AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
                //.AddNewtonsoftJson(options => { });
                ;

            // NHibernate session registration
            // Note that we are registring SessionFactory as a Singleton
            // but session is registered per request
            var nHibernateConfiguration = new DBSession().GetConfiguration();
            var nHibernateSessionFactory = new DBSession().GetSessionFactory(nHibernateConfiguration);
            services.AddSingleton(nHibernateConfiguration);
            services.AddSingleton(nHibernateSessionFactory);
            services.AddScoped(factory => nHibernateSessionFactory.OpenSession());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
