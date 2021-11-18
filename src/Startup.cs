using Amazon.DynamoDBv2;
using Amazon.SQS;
using DynamoDBApi.Reporitories;
using DynamoDBApi.Services;
using DynamoDBApi.SqsLogger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace DynamoDBApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services/*, IAmazonSQS sqsClient*/)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddSingleton<IBookService, BookService>();
            services.AddSingleton<IDynamoDbRepository, DynamoDbRepository>();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddAWSService<IAmazonSQS>();

            services.AddLogging();

            //var queueName = GetQueueName(Configuration);
            //services.AddLogging(l => l.AddProvider(new LoggerProvider(new LoggerConfig(), queueName, sqsClient)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IAmazonSQS sqsClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //to remove:
            var queueName = GetQueueName(Configuration);
            loggerFactory.AddProvider(new LoggerProvider(new LoggerConfig(), queueName, sqsClient));
        }

        private string GetQueueName(IConfiguration configuration)
        {
            return configuration.GetSection("AWS")?.GetValue<string>("SqsQueueName");
        }
    }
}
