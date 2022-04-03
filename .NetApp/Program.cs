global using System;
global using Newtonsoft.Json;
global using Python.Runtime;
global using System.Collections.Generic;
global using System.Linq;
global using project.Src;
global using project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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
            ValueNetwork.LoadModel();
            ValueNetwork.ConsumeMovesFromDB();
            SelfPlay.Run(10);
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
