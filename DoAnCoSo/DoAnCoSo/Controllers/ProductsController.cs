using Azure;
using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace DoAnCoSo.Controllers
{
    public class ProductsController : Controller
    {
        DataDoAnCoSoContext db = new DataDoAnCoSoContext();
        private readonly DataDoAnCoSoContext _context;

        public ProductsController(DataDoAnCoSoContext context)
        {
            _context = context;
        }
        public IActionResult Products(int? page)
        {
            int pageNumber = page  == null || page <= 0 ? 1 : page.Value;
            int pageSize = 6;
            var lsproducts = db.Products
                .AsNoTracking()
                .OrderBy(x => x.ProId)
                .ToList();
               // .ToPagedList(pageNumber, pageSize);

            PagedList<Product> lstpro = new PagedList<Product>(lsproducts.AsQueryable(), pageSize, pageNumber);
            ViewBag.CurrentPage = pageNumber; // Để giữ giá trị phân loại trong view
            return View(lsproducts);
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProId == id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
