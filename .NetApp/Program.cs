using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Python.Runtime;
using System.IO;

namespace project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.CreateDirectory("data/journal");
            Directory.CreateDirectory("data/DB");
            Directory.CreateDirectory("data/model");
            PythonEngine.Initialize();
            PythonEngine.BeginAllowThreads();
            AI.LoadModel();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://+:5000");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
