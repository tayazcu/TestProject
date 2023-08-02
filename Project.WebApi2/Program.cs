using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Project.WebApi2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration(builder =>
                    {
                        builder.AddEnvironmentVariables(prefix: "PROJECT_WEBAPI2_");
                    })
                    .ConfigureLogging((hostContext, logging) =>
                    {
                        logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                    })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                        webBuilder.ConfigureKestrel(options =>
                        {
                            options.Listen(IPAddress.Any, 5002, listenOptions =>
                            {
                                listenOptions.Protocols = HttpProtocols.Http2;
                            });
                            options.Listen(IPAddress.Any, 5003, listenOptions =>
                            {
                                listenOptions.Protocols = HttpProtocols.Http1;
                            });
                        });
                    });
    }
}
