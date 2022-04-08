using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace MyFinances
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host;
            if (args.Contains("--runAsService"))
            {
                host = CreateWebHostServiceBuilder(args).Build();
                host.RunAsService();
            }

            if (args.Contains("--runAsDebianService"))
            {
                host = CreateWebHostDebianServiceBuilder(args).Build();
                host.Run();
            }

            host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                 .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                     logging.AddConsole().SetMinimumLevel(LogLevel.Information);
                     logging.AddEventSourceLogger();
                 })
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls($"http://{BuildUrl(args)}:{GetPort(args)}");
        }

        public static IWebHostBuilder CreateWebHostDebianServiceBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .UseStartup<Startup>()
                .UseUrls($"http://{BuildUrl(args)}:{GetPort(args)}");
        }

        public static IWebHostBuilder CreateWebHostServiceBuilder(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls($"http://{BuildUrl(args)}:{GetPort(args)}");
        }

        private static string GetPort(string[] args)
        {
            var defaultPort = "29000";

            if (!args.Contains("--port"))
            {
                return defaultPort;
            }

            var portIndex = Array.IndexOf(args, "--port") + 1;
            var port = args[portIndex];
            return port;
        }

        private static string BuildUrl(string[] args)
        {
            var defaultServerName = "localhost";

            if (!args.Contains("--servername"))
            {
                return defaultServerName;
            }

            var nameIndex = Array.IndexOf(args, "--servername") + 1;
            var name = args[nameIndex];
            return name;
        }
    }
}
