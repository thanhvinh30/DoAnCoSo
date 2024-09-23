using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



namespace DoAnCoSo.Respository
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> categories { get; set; }
    }
}
