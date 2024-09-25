using DoAnCoSo.Extension;
using DoAnCoSo.Models;
using DoAnCoSo.Respository;
using Microsoft.AspNetCore.Mvc;


namespace DoAnCoSo.Controllers.Components
{
    public class HeaderCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<Cart> cartItems = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
            var session = HttpContext.Session.Get<List<Cart>>("Cart");
            if (session != null)
            {
                cartItems = session;
            }
            return View(session);

        }
    }
}
