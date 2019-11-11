
namespace ORMWebAPI
{
    using System.IO;
    using Ciena.BluePlanet.BpWeb;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseBpServer()
                .UseBpLogging()
                .UseKestrel(options =>
                {
                    //Setting MaxRequestBodySize to null to accept large files in POST request such as in cases of raw TPE FRE to 1P conversion
                    options.Limits.MaxRequestBodySize = null;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
