using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DoAnCoSo.ModelView;                   // Mới Thêm
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Controllers
{
    public class HomeController : Controller
    {
        DataDoAnCoSoContext db = new DataDoAnCoSoContext();
        private readonly DataDoAnCoSoContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataDoAnCoSoContext context)
        {
            _logger = logger;
            _context = context;                         // Add
        }
        public IActionResult MyAccount() 
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login", "Customer");
        }
        public async Task<IActionResult> Index()
        {
            // Bắt đầu
            var categoriesWithProductCount = await _context.Categories
                .Select(c => new
                {
                    c.CatId,
                    c.CatName,
                    ProductCount = _context.Products.Count(p => p.CatId == c.CatId)
                }).ToListAsync();

            HomeViewVM vm = new HomeViewVM();

            var lsproducts = await _context.Products
                .AsNoTracking()
                .Where(x => x.Active == true && x.HomeFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();

            List<ProductHomeVM> lsProductsView = new List<ProductHomeVM>();

            var lsCats = await _context.Categories
                .AsNoTracking()
                .Where(x => x.Published == true && x.ParentId == 0)
                .OrderByDescending(x => x.Ordering)
                .ToListAsync();

            foreach (var item in lsCats)
            {
                ProductHomeVM productHomeVM = new ProductHomeVM();
                productHomeVM.lsCategory = item;
                productHomeVM.lsProducts = lsproducts
                    .Where(x => x.CatId == item.CatId)
                    .ToList();
                lsProductsView.Add(productHomeVM);
            }

            var bestSellerProducts = await _context.Products
                .AsNoTracking()
                .Where(p => p.Active == true && p.BestSellers == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(6)
                .ToListAsync();

            vm.Products = lsProductsView;
            vm.Categories = lsCats;

            ViewBag.AllProducts = lsproducts;
            ViewBag.lsCat = lsCats;
            ViewBag.BestSellerProducts = bestSellerProducts;

            return View(vm);
        }



        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Warranty_Policy()
        {
            return View();
        }
        public IActionResult Return_Policy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if(statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            
        }
    }
}
