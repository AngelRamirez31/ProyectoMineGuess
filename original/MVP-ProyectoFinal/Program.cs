using MVP_ProyectoFinal.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var contentRoot = Directory.GetCurrentDirectory();
var webRoot = Path.Combine(contentRoot, "wwwroot");
Directory.CreateDirectory(webRoot);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
    });


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

var useApi = builder.Configuration.GetSection("Api").GetValue<bool>("UseApi");
var apiBase = builder.Configuration.GetSection("Api").GetValue<string>("ApiBaseUrl");
RepositorioBloques.ConfigurarApi(useApi, apiBase);
RepositorioEntidades.ConfigurarApi(useApi, apiBase);
RepositorioBiomas.ConfigurarApi(useApi, apiBase);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
