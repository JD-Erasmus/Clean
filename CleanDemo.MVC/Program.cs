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

// Configure custom audit provider
var eventFolderPath = builder.Configuration.GetValue<string>("EventFolderPath");
var logFileName = $"{DateTime.Now:yyyyMMdd}_audit.log";
var logFilePath = Path.Combine(eventFolderPath, logFileName);

// Ensure the audit log directory exists
if (!Directory.Exists(eventFolderPath))
{
    Directory.CreateDirectory(eventFolderPath);
}

// Use custom audit data provider
Audit.Core.Configuration.Setup()
    .UseCustomProvider(new CustomAuditDataProvider(logFilePath));

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
