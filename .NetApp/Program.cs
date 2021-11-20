using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.CreateDirectory("data/journal");
            Directory.CreateDirectory("data/model");
            //AIModel.LoadModel();
            //AIModel.start();
            CreateHostBuilder(args).Build().StartAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
