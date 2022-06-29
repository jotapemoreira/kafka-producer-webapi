using Kafka.Producer.Application.Services;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Reflection;

namespace Kafka.Producer.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = version.ProductVersion,
                    Title = "Produtor de mensagens com kafka",
                    Description = "API que produz mensagens utilizando kafka.",
                    Contact = new OpenApiContact { Name = "Jotape Moreira" }
                });

                c.CustomSchemaIds(x => x.FullName);
                c.CustomSchemaIds((Type currentClass) =>
                {
                    var returnedValue = currentClass.Name;

                    if (returnedValue.EndsWith("Dto", StringComparison.InvariantCultureIgnoreCase))
                    {
                        returnedValue = returnedValue.Replace("Dto", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                    }

                    return returnedValue;
                });
            });

            services.AddHealthChecks();

            //services.AddKafkaProducer(Configuration);

            services.AddScoped<IProducerService, ProducerService>();

        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseHttpLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health");

            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseAuthorization();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}