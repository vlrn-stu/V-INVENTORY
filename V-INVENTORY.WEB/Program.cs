using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Services;
using V_INVENTORY.WEB;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#region Services

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var baseAddress = builder.Configuration.GetSection("Api:Inventory:Url").Value;

builder.Services.AddHttpClient("InventoryAPI", client =>
{
	client.BaseAddress = new Uri(baseAddress);
});

builder.Services.AddSingleton<InventoryItemService>();
builder.Services.AddSingleton<InventoryItemLocationService>();
builder.Services.AddSingleton<InventoryItemImageService>();

#endregion Services

await builder.Build().RunAsync();