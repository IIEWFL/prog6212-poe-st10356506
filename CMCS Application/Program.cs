using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CMCS_Application.Models;
using System.Globalization;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add FluentValidation
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AutoReview>());

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session and distributed cache services
builder.Services.AddDistributedMemoryCache(); // Adds in-memory storage for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true; // Makes cookies HTTP-only
    options.Cookie.IsEssential = true; // Ensures cookies are essential for the app
});

// Configure cookies for authentication and authorization
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Login"; // Redirect to login page
        options.AccessDeniedPath = "/AccessDenied"; // Redirect to access denied page
        options.Cookie.Name = "UserCookiesDontEat"; // Cookie name
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Expiration time
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Add middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseSession(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
