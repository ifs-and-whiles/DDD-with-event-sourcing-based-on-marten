using System.Security.Claims;
using Autofac;
using DDDWithEventSourcingBasedOnMarten.Expenses;
using DDDWithEventSourcingBasedOnMarten.Expenses.API;
using DDDWithEventSourcingBasedOnMarten.Infrastructure.Config;
using DDDWithEventSourcingBasedOnMarten.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DDDWithEventSourcingBasedOnMarten
{
    public class Startup
    {
        private  IConfiguration _configuration;
        public Startup()
        {}

        public void ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            _configuration = (IConfiguration) serviceProvider.GetService(typeof(IConfiguration));

            services.AddHttpClient();
            services.AddControllers();
            
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo()
                        {
                            Title = "DDDWithEventSourcingBasedOnMarten",
                            Version = "v1"
                        }
                    );
                    c.CustomSchemaIds(x => x.FullName);
                }
            );
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration);
            
            var applicationConfig = _configuration.GetSection(ConfigKeys.Application).Get<ApplicationConfig>();
            builder.RegisterInstance(_configuration.GetSection(ConfigKeys.Database).Get<DatabaseConfig>());

            builder.RegisterInstance(applicationConfig);
            builder.RegisterModule(new AutofacModule());
            builder.RegisterModule(new ExpensesModule());

            RegisterHostedServices(builder, applicationConfig);
        }

        private void RegisterHostedServices(ContainerBuilder builder, ApplicationConfig applicationConfig)
        {
            if (applicationConfig.RunProjections)
                builder.RegisterType<MartenProjectionsHost>()
                    .As<IHostedService>()
                    .AsSelf()
                    .SingleInstance();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //TODO:[FP] what to do
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMiddleware<ExpensesApiErrorHandlingMiddleware>();
            
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            
            app.UseSwagger();

            app.UseSwaggerUI(
                c => c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json", "Nacoposzlo v1"
                )
            );
        }
    }
}