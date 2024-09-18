using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using DoAnCoSo.Helpper;

namespace DoAnCoSo.Controllers
{
    public class AccountController : Controller
    {
        DataDoAnCoSoContext db = new DataDoAnCoSoContext();
        private readonly DataDoAnCoSoContext _context;

        public AccountController(DataDoAnCoSoContext context)
        {
            _context = context;
        }
        public IActionResult Account()
        {
            return View();
        }
    }
}
