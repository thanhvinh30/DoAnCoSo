using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



namespace DoAnCoSo.Respository
{
    public class DataContext : IdentityDbContext<Customer>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
