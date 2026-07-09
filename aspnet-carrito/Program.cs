using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 1. Registrar el almacenamiento en memoria para sesiones
builder.Services.AddDistributedMemoryCache();

// 2. Configurar la sesión con cookie segura
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;                        // flag HttpOnly
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // flag Secure (requiere HTTPS)
    options.Cookie.SameSite = SameSiteMode.Strict;           // flag SameSite
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 3. Habilitar el middleware de sesión (ANTES de MapControllers)
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Carrito}/{action=Index}/{id?}");

app.Run();
