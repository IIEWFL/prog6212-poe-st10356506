using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CMCS_Application.Models;
using System.Globalization;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//add fluent validation 
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AutoReview>());

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Session timeout 
    options.Cookie.HttpOnly = true; // Prevent client-side script access
    options.Cookie.IsEssential = true; // Ensure compatibility
});


var app = builder.Build();

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
