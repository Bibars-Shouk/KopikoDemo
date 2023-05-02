using BlazorKopikoDemoClient;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorKopikoDemoClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Set Grpc Channel
            builder.Services.AddSingleton(sp => { return GrpcChannel.ForAddress("https://localhost:7027", new GrpcChannelOptions { HttpHandler = new GrpcWebHandler(new HttpClientHandler()) }); });

            await builder.Build().RunAsync();
        }
    }
}