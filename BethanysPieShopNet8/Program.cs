using BethanysPieShopNet8.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<IPieRepository, MockPieRepository>(); // For dependency injection
builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>(); // For dependency injection

builder.Services.AddControllersWithViews(); // For MVC configuration


var app = builder.Build();

app.UseStaticFiles();// For static files

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapDefaultControllerRoute();

app.Run();
