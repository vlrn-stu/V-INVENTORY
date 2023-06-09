using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using VULTIME.Core.Data.Notifications;
using VULTIME.VINV.API.DB;
using VULTIME.VINV.API.Statisctics;
using VULTIME.VINV.Common.Models;

var builder = WebApplication.CreateBuilder(args);

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
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    dbContext.Database.Migrate();
}

app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<InventoryItem>("InventoryItems");
    builder.EntitySet<InventoryItemLocation>("InventoryItemLocations");
    return builder.GetEdmModel();
}