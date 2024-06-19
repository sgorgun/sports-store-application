﻿using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using SportsStore.Models.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(SessionCart.GetCart);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

app.UseStaticFiles();

app.UseSession();


app.MapControllerRoute(
     name: "pagination",
     pattern: "Products/Page{productPage:int}",
     defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute(
    name: "categoryPage",
    pattern: "{category}/Page{productPage:int}",
    defaults: new { Controller = "Home", action = "Index" });

app.MapControllerRoute(
      name: "category",
      pattern: "Products/{category}",
      defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute(
      name: "shoppingCart",
      pattern: "Cart",
      defaults: new { Controller = "Cart", action = "Index" });

app.MapControllerRoute(
    "default",
    "/",
    new { Controller = "Home", action = "Index" });
 
app.MapControllerRoute(
      "checkout",
      "Checkout",
      new { Controller = "Order", action = "Checkout" });

app.MapControllerRoute(
    "remove",
    "Remove",
    new { Controller = "Cart", action = "Remove" });




app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);

app.Run();
