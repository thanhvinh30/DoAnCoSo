using DoAnCoSo.Helpper;
using DoAnCoSo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;





var builder = WebApplication.CreateBuilder(args);


builder.Services.AddIdentity<Customer, IdentityRole>()
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



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataDoAnCoSoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbPhuTungXeMay")));


builder.Services.AddDistributedMemoryCache();



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
    //options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseSession();

app.UseStaticFiles();

app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
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



