using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CMCS_Application.Models;
using System.Globalization;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//add FluentValidation
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AutoReview>());

//add services to the container.
builder.Services.AddControllersWithViews();

//add session and distributed cache services
builder.Services.AddDistributedMemoryCache(); //adds in-memory storage for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); //session timeout
    options.Cookie.HttpOnly = true; //makes cookies HTTP-only
    options.Cookie.IsEssential = true; //ensures cookies are essential for the app
});

//configure cookies for authentication and authorization
//https://stackoverflow.com/questions/17769011/how-does-cookie-based-authentication-work
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Login"; //redirect to login page
        options.AccessDeniedPath = "/AccessDenied"; //redirect to access denied page
        options.Cookie.Name = "UserCookiesDontEat"; //cookie name
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); //expiration time
    });
builder.Services.AddAuthorization();

var app = builder.Build();

//add middleware
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
