using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VULTIME.VINV.WEB;
using VULTIME.VINV.WEB.Services;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#region Services

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

string inventoryBaseUrl = builder.Configuration.GetSection("Api:Inventory:Url").Value ?? "https://localhost:7265/";

builder.Services.AddHttpClient("InventoryAPI", client =>
{
    client.BaseAddress = new Uri(inventoryBaseUrl);
});

builder.Services.AddSingleton<InventoryItemService>();
builder.Services.AddSingleton<InventoryItemLocationService>();
builder.Services.AddSingleton<InventoryItemImageService>();
builder.Services.AddSingleton<InventoryItemStatisticsService>();

#endregion Services

await builder.Build().RunAsync();