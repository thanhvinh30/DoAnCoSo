using DoAnCoSo.Helpper;
using DoAnCoSo.Models;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataDoAnCoSoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbPhuTungXeMay")));



var app = builder.Build();
app.UseStaticFiles();

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



// Tự thêm vào 

app.MapPost("/create-product", async (Product product, IFormFile fThumb) =>
{
    // Chuy?n ??i tên s?n ph?m thành d?ng Title Case
    product.ProName = Utilities.ToTitleCase(product.ProName);

    // Ki?m tra file ?nh có t?n t?i không
    if (fThumb != null)
    {
        string extension = Path.GetExtension(fThumb.FileName);
        string image = Utilities.SEOUrl(product.ProName) + extension;

        // T?i ?nh lên và l?y ???ng d?n ?nh
        product.ProImage = await Utilities.UploadFile(fThumb, "img-PhuTungXe(BanMoi)", image.ToLower());

        // N?u không có ?nh, ??t giá tr? m?c ??nh
        if (string.IsNullOrEmpty(product.ProImage))
        {
            product.ProImage = "default.jpg";
        }
    }

    // Thêm thông tin ngày t?o và ngày s?a
    product.DateCreated = DateTime.Now;
    product.DateModified = DateTime.Now;

    // Thêm s?n ph?m vào c? s? d? li?u (logic database)
    // Ví d?: _dbContext.Products.Add(product);
    // await _dbContext.SaveChangesAsync();

    return Results.Ok(product);
});