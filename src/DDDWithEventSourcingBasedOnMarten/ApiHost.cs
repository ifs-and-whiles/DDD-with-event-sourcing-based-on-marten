﻿using System.Net;
using Autofac.Extensions.DependencyInjection;
using DDDWithEventSourcingBasedOnMarten.Infrastructure.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DDDWithEventSourcingBasedOnMarten
{
    public class ApiHost
    {
        public static void Run(IConfiguration config)
        {
            var applicationConfig = config.GetSection(ConfigKeys.Application).Get<ApplicationConfig>();

            CreateHost(config, applicationConfig.Port).Run();
        }

        public static IHost CreateHost(IConfiguration config, int port) =>
            Host.CreateDefaultBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(config);
                })

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls($"http://*:{port}")
                        .ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.Listen(IPAddress.Any, 5000);
                        })
                        .UseStartup<Startup>()
                        .UseSerilog();

                })
                .Build();
    }
}