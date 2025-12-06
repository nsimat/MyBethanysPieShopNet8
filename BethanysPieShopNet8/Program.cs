using BethanysPieShopNet8.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure services in the container
builder.Services.AddScoped<IPieRepository, PieRepository>(); // For dependency injection
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); // For dependency injection
builder.Services.AddScoped<IOrderRepository, OrderRepository>(); // For dependency injection
builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp)); // For dependency injection

builder.Services.AddSession(); // For session state

builder.Services.AddHttpContextAccessor(); // For session state

builder.Services.AddControllersWithViews(); // For MVC configuration

builder.Services.AddRazorPages(); // For Razor Pages configuration

builder.Services.AddOpenApi();

// Add secrets.json to the configuration and database connection
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<BethanysPieShopDbContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("BethanysPieShopDbContext")));

builder.Services.AddControllers(). // For API configuration
    AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true; // Optional: for better readability

    });

// Add Swagger
builder.Services.AddEndpointsApiExplorer();// For API configuration(ajout)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Bethany's Pie Shop API", Version = "v1" });
});

var app = builder.Build();

// Configure middleware & HTTP request pipeline

app.UseStaticFiles();// For static files

app.UseSession(); // For session state

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapDefaultControllerRoute(); // "{controller=Home}/{action=Index}/{id?}"
                                 // app.MapControllerRoute(
                                 // name: "default",
                                 // pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // For Razor Pages

DbInitializer.Seed(app); // Seed the database

await app.RunAsync();

Console.WriteLine("Bethany's Pie Shop Web App has been stopped...");
