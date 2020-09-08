using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System;
using InfluxReaderBlazor.Shared;
using DataAccess;

namespace InfluxReaderBlazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
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
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
