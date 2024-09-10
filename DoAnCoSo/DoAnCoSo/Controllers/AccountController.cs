using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSo.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Account()
        {
            return View();
        }
    }
}
