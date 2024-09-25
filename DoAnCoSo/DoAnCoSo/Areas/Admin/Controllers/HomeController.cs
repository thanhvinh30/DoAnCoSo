using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {

        //Chức Năng thông báo Start
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            switch (type)
            {
                case "Success":
                    TempData["AlertType"] = "alert-Success"; break;
                case "Warning":
                    TempData["AlertType"] = "alert-Warning"; break;
                case "Error":
                    TempData["AlertType"] = "alert-Error"; break;
                default: TempData["AlertType"] = ""; break;
            }
        }
        //Chức Năng thông báo End

        public IActionResult Index()
        {
            return View();
        }
    }
}
