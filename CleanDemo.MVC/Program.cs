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


var builder = WebApplication.CreateBuilder(args);
void RegisterServices(IServiceCollection services)
{
    DependencyContainer.RegisterServices(services);
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieDbContext")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDbContext")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AuthDbContext>();


// Configure the audit to use the custom provider
var eventFolderPath = builder.Configuration.GetValue<string>("EventFolderPath");
var logFileName = $"{DateTime.Now:yyyyMMdd}_audit.log";
var logFilePath = Path.Combine(eventFolderPath, logFileName);

// Ensure the directory exists
if (!Directory.Exists(eventFolderPath))
{
    Directory.CreateDirectory(eventFolderPath);
}

// Use the custom audit data provider
Audit.Core.Configuration.Setup()
    .UseCustomProvider(new CustomAuditDataProvider(logFilePath));

RegisterServices(builder.Services);
var app = builder.Build();


// Run seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        // Log errors or take other appropriate actions
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
