
using DoAnCoSo.Extension;
using DoAnCoSo.Models;
using DoAnCoSo.Respository;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSo.Controllers.Components
{
    public class NumberCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            //List<Cart> cartItems = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
            var cart = HttpContext.Session.Get<List<Cart>>("Cart");
            return View(cart);
        }
    }
}
