using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using VULTIME.Core.Data.Notifications;
using VULTIME.VINV.API.DB;
using VULTIME.VINV.API.Statisctics;
using VULTIME.VINV.Common.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<InventoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<INotificationManager, NotificationManager>();
builder.Services.AddSingleton<InventoryItemStatisticsProvider>();

builder.Services.AddControllers()
    .AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(100)
    .AddRouteComponents("odata", GetEdmModel()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        _ = builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
    _ = app.UseCors();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    InventoryDbContext dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    dbContext.Database.Migrate();
}

app.Run();

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    _ = builder.EntitySet<InventoryItem>("InventoryItems");
    _ = builder.EntitySet<InventoryItemLocation>("InventoryItemLocations");
    return builder.GetEdmModel();
}