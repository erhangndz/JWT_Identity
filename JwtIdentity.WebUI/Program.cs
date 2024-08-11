using JwtIdentity.WebUI.Handlers;
using JwtIdentity.WebUI.Services.ProtectedService;
using JwtIdentity.WebUI.Services.UserServices;
using JwtIdentity.WebUI.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed((host) => true)
               .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Login/Index";
    opt.LogoutPath = "/Login/Logout";
    opt.AccessDeniedPath = "/Pages/AccessDenied";
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.Cookie.Name = "IdentityJwt";
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Login/Index";
    opt.ExpireTimeSpan = TimeSpan.FromDays(5);
    opt.Cookie.Name = "IdentityCookie";
    opt.SlidingExpiration = true;
});



// Add services to the container.

builder.Services.AddTransient<AuthenticationHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(nameof(ApiSettings)));
var settings = builder.Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>();
builder.Services.AddHttpClient<IUserService, UserService>(client =>
{
    client.BaseAddress = new Uri(settings.BaseUrl);
});

builder.Services.AddHttpClient<IProtectedService, ProtectedService>(client =>
{

    client.BaseAddress = new Uri(settings.BaseUrl);
}).AddHttpMessageHandler<AuthenticationHandler>();
builder.Services.AddControllersWithViews();
    

builder.Services.AddDistributedMemoryCache();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
