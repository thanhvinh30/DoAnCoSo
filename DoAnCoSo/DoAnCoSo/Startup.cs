using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;


namespace DoAnCoSo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            configuration = configuration;

        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ phân tán để lưu trữ session
            services.AddControllersWithViews(); // Hoặc AddMvc() tùy theo cấu hình của bạn
            // Thêm mới
            services.AddMvc();
            services.AddHttpContextAccessor();
            // End

            //
            var stringConnectdb = Configuration.GetConnectionString("dbPhuTungXeMay");
            services.AddDbContext<DataDoAnCoSoContext>(Options => Options.UseSqlServer(stringConnectdb));
            //

            //
            services.AddDbContext<DataDoAnCoSoContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("dbPhuTungXeMay")));
            //

            //services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRange.All }));                // Sửa phần chữ Unicode ( tiếng việt ) 

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //services.AddDbContext<DataDoAnCoSoContext>(ServiceLifetime.Transient);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                 );
            });

            // tạo mới Logger từ CHatGPT ( xóa nhớ để ý phần public void đã có thêm vào logger )
            logger.LogInformation("Application has started and routing is configured correctly.");
        }
    }
}
