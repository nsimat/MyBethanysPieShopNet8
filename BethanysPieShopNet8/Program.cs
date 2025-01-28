using BethanysPieShopNet8.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<IPieRepository, PieRepository>(); // For dependency injection
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); // For dependency injection

builder.Services.AddControllersWithViews(); // For MVC configuration
builder.Services.AddDbContext<BethanysPieShopDbContext>(options =>

 options.UseSqlServer(builder.Configuration.GetConnectionString("BethanysPieShopDbContext")));

var app = builder.Build();

app.UseStaticFiles();// For static files

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapDefaultControllerRoute(); // "{controller=Home}/{action=Index}/{id?}"
                                 //app.MapControllerRoute(
                                 //name: "default",
                                 //pattern: "{controller=Home}/{action=Index}/{id?}");

DbInitializer.Seed(app); // Seed the database

app.Run();
