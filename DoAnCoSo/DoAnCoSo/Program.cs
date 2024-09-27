using DoAnCoSo.Helpper;
using DoAnCoSo.Models;
using DoAnCoSo.Respository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/Customer/Login";
        options.LogoutPath = "/Customer/Logout";
        options.AccessDeniedPath = "/Customer/AccessDenied";
    });
builder.Services.AddAuthorization();

builder.Services.AddIdentity<AppUserModel, IdentityRole>()
    .AddEntityFrameworkStores<DataDoAnCoSoContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;


    options.User.RequireUniqueEmail = true;
});

builder.Services.AddDbContext<DataDoAnCoSoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbPhuTungXeMay")));





var app = builder.Build();


app.UseRouting();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();  // Session phải được đặt trước Authentication
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
//logger.LogInformation("Application has started and routing is configured correctly.");


