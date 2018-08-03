using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MvcHybrid
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);
            var Configuration = config.Build();

            BuildWebHost(args, Configuration).Run();
        }

        public static IWebHost BuildWebHost(string[] args, IConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(configuration.GetSection("Host")["LocalHost"])
                .UseStartup<Startup>()
                .Build();
    }
}
