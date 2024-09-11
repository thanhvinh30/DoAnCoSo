using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataDoAnCoSoContext _context;

        public ProductsController(DataDoAnCoSoContext context)
        {
            _context = context;
        }
        public IActionResult Products()
        {
            return View();
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
