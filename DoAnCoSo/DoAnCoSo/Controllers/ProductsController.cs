using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSo.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Products()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
