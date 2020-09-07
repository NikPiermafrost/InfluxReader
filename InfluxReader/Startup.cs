using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InfluxReader
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                                  });
            });
            var address = Configuration.GetSection("ConfigurationParams").GetSection("ipAddress").Value;
            var usrName = Configuration.GetSection("ConfigurationParams").GetSection("usrName").Value;
            var password = Configuration.GetSection("ConfigurationParams").GetSection("password").Value;
            var dbName = Configuration.GetSection("ConfigurationParams").GetSection("dbName").Value;
            var rabbitConf = new ConfigurationModel()
            {
                Exchange = Configuration.GetSection("RabbitSender").GetSection("exchange").Value,
                RoutingKey = Configuration.GetSection("RabbitSender").GetSection("routingKey").Value,
                Queue = Configuration.GetSection("RabbitSender").GetSection("queue").Value,
                Hostname = Configuration.GetSection("RabbitSender").GetSection("hostname").Value,
                UserName = Configuration.GetSection("RabbitSender").GetSection("userName").Value,
                Password = Configuration.GetSection("RabbitSender").GetSection("password").Value,
                Vhost = Configuration.GetSection("RabbitSender").GetSection("vhost").Value,
                Port = Convert.ToInt32(Configuration.GetSection("RabbitSender").GetSection("port").Value)
            };
            services.AddTransient<IInfluxDataAccessService>(s => new InfluxDataAccessService(address, usrName, password, dbName));
            services.AddScoped<IRabbitSender>(s => new RabbitSender(rabbitConf));
            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "InfluxReader");
            });

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
