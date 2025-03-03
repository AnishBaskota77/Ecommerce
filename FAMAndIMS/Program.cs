using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using AutoMapper;
using FAMAndIMS.Data.Common;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Extension;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Register DbConnectionManager as a service for dependency injection
builder.Services.AddSingleton<DBConnectionManager>();

// Add AutoMapper services
builder.Services.AddAutoMapper(typeof(MappingProfiles));


////for notification
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.TopRight;
});

//add application services to service extension
builder.Services.AddApplicationServices();

builder.Services.Configure<IISOptions>(options =>
{
    options.AutomaticAuthentication = false;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {

        o.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        o.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = context =>
            {
                if (context.Request.Query.TryGetValue("returnUrl", out var returnUrl))
                {
                    // Store the returnUrl in the authentication ticket
                    context.Properties.RedirectUri = returnUrl;
                }
                return Task.CompletedTask;
            },
            OnRedirectToLogin = context =>
            {
                if (!context.Request.Path.StartsWithSegments("/Account/Login"))
                {
                    // Store the original request path as the custom return URL
                    context.Response.Redirect("/AdminLogin/AdminLoginPage?returnUrl=" + Uri.EscapeDataString(context.Request.Path));
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();
app.UseNotyf();


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

    pattern: "{controller=AdminLogin}/{action=AdminLoginPage}/{id?}");


app.Run();
