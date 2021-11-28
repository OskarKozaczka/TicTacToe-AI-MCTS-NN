using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Python.Runtime;

namespace project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.CreateDirectory("data/journal");
            Directory.CreateDirectory("data/model");
            PythonEngine.Initialize();
            PythonEngine.BeginAllowThreads();
            AIModel.TrainModel();
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
