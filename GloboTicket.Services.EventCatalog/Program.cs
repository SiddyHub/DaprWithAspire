using Dapr.Client;
using Dapr.Extensions.Configuration;
using GloboTicket.Services.EventCatalog;
using GloboTicket.Services.EventCatalog.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using (var scope = scopeFactory.CreateScope())
{
    var dbCosmos = scope.ServiceProvider.GetRequiredService<EventCatalogCosmosDbContext>();

    //dbCosmos.Database.EnsureDeleted();
    dbCosmos.Database.EnsureCreated();
}

startup.Configure(app, app.Environment);

app.MapDefaultEndpoints();

app.Run();
