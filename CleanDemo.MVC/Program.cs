using Clean.Infra.Data.Context;
using Clean.Infrastructure.IoC;
using Clean.Infra.Data.Seed;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Clean.MVC.MappingProfiles;
using System;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CleanDemo.MVC.Audit;
using Microsoft.Extensions.Logging;
using Audit.Core;
using Audit.SqlServer.Providers;
using System.Data.SqlClient;
using Audit.SqlServer;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Register services method
void RegisterServices(IServiceCollection services)
{
    DependencyContainer.RegisterServices(services);
}

// ========================================
// Configure Services
// ========================================

// Add controllers with views
builder.Services.AddControllersWithViews();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure database contexts
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieDbContext")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDbContext")));

// Configure Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AuthDbContext>();


// ========================================
// Configure Audit Trail
// ========================================

string auditConnectionString = builder.Configuration.GetConnectionString("MovieDbContext");
Audit.Core.Configuration.DataProvider = new SqlDataProvider()
{
    ConnectionString = auditConnectionString,
    Schema = "dbo",
    TableName = "Event",
    IdColumnName = "EventId",
    JsonColumnName = "JsonData",
    LastUpdatedDateColumnName = "LastUpdatedDate",
    CustomColumns = new List<CustomColumn>()
    {
        new CustomColumn("EventType", ev => ev.EventType),
        new CustomColumn("Username", ev => GetJsonValue(ev, "Action", "UserName")),
        new CustomColumn("IpAddress", ev => GetJsonValue(ev, "Action", "IpAddress")),
        new CustomColumn("ActionParameters", ev => GetJsonValue(ev, "Action", "ActionParameters"))
    }
};

// Function to extract JSON value using path
string GetJsonValue(AuditEvent ev, string pathToAction, string propertyName)
{
    var jsonData = ev.ToJson();
    var jsonObject = JObject.Parse(jsonData);
    var action = jsonObject[pathToAction];
    return action?[propertyName]?.ToString();
}

// Register application services
RegisterServices(builder.Services);

var app = builder.Build();

// ========================================
// Seed Data (Development Only)
// ========================================
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            SeedData.Initialize(services);
        }
        catch (Exception ex)
        {
            // Log seeding errors
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}

// ========================================
// Configure the HTTP Request Pipeline
// ========================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configure default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


