using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorTetris;
using BlazorTetris.Services;
using Fluxor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ITetrisJs, TetrisJs>();

builder.Services.AddFluxor(options => 
{
    options.ScanAssemblies(Assembly.GetExecutingAssembly());
    options.UseReduxDevTools();
});

await builder.Build().RunAsync();
