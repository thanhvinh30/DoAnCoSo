﻿using DoAnCoSo.Models;
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

        public IActionResult Index()
        {
            //Start
            // Tính toán số lượng sản phẩm theo từng danh mục
            var categoriesWithProductCount = _context.Categories
            .Select(c => new
            {
                c.CatId,
                c.CatName,
                ProductCount = _context.Products.Count(p => p.CatId == c.CatId)
            }).ToList();
            // Start
            HomeViewVM vm = new HomeViewVM();

            var lsproducts = _context.Products
                                                .AsNoTracking ()
                                                .Where( x => x.Active == true  && x.HomeFlag == true)
                                                .OrderByDescending ( x => x.DateCreated )
                                                .ToList ();


            List<ProductHomeVM> lsProductsView = new List<ProductHomeVM> ();

            var lsCats = _context.Categories
                                            .AsNoTracking ()
                                            .Where( x => x.Published == true && x.ParentId == 0)
                                            .OrderByDescending (x => x.Ordering)
                                            .ToList ();


            foreach(var item in lsCats)
            {
                ProductHomeVM productHomeVM = new ProductHomeVM();
                productHomeVM.lsCategory = item;
                productHomeVM.lsProducts = lsproducts
                                                        .Where( x => x.CatId == item.CatId)
                                                        .ToList ();
                lsProductsView.Add (productHomeVM);
            }
            
            vm.Products = lsProductsView;
            vm.Categories = lsCats;
            ViewBag.AllProducts = lsproducts;
            ViewBag.lsCat = lsCats;
            //End
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
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
