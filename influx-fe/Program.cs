using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using influx_fe.Services;
using System.Net.Http.Json;
using influx_fe.Models;

namespace influx_fe
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("app");

            var client = new HttpClient() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)};
            builder.Services.AddScoped(sp => client);
            var jsonRequest = await client.GetAsync("appSettings.json");
            var rabbitConfiguration = await jsonRequest.Content.ReadFromJsonAsync<ConfigurationModel>();
            builder.Services.AddScoped<IRabbitSender>(rService => new RabbitSender(rabbitConfiguration));

            await builder.Build().RunAsync();
        }
    }
}
