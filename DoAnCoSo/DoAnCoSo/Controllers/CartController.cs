using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using DoAnCoSo.Respository;
using Microsoft.AspNetCore.Mvc.Rendering;
using DoAnCoSo.Models.ViewModels;
using NuGet.Protocol;

namespace DoAnCoSo.Controllers
{
    public class CartController : Controller
    {
        DataDoAnCoSoContext db = new DataDoAnCoSoContext();
        private readonly DataDoAnCoSoContext _context;

        public CartController(DataDoAnCoSoContext context)
        {
            _context = context;
        }
        public IActionResult Cart()
        {
            List<Cart> cartItems = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
            CartItemViewModel cart = new()
            {
                CartItems = cartItems,
                GrandToTal = cartItems.Sum( x => x.Quantity*x.Price)
            };

            return View(cart);
        }
        public async Task<IActionResult> Add(int Id)
        {
            Product pro = await _context.Products.FindAsync(Id);
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
            Cart cartitem = cart.Where(x => x.ProId == Id).FirstOrDefault();
            //Cart cartItem = cart.FirstOrDefault(x => x.ProId == id);
            if (cartitem == null)
            {
                cart.Add(new Cart(pro));
            }
            else
            {
                cartitem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            return Redirect(Request.Headers["Referer"].ToString());
        }


        //Action để tăng số lượng sản phẩm
        public IActionResult Increase(int id)
        {
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
            Cart cartItem = cart.Where(c => c.ProId == id).FirstOrDefault();
            if (cartItem != null && cartItem.Quantity > 0)
            {
                ++cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProId == id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Cart");
        }

        // Action để giảm số lượng sản phẩm
        public IActionResult Decrease(int id)
        {
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
            Cart cartItem = cart.Where( c => c.ProId == id).FirstOrDefault();
            if (cartItem != null && cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll( p => p.ProId == id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Cart");
        }

        // Action để xóa sản phẩm khỏi giỏ hàng
        public IActionResult Remove(int id)
        {
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
            cart.RemoveAll(p => p.ProId == id);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Cart");
        }
        public IActionResult CleanCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Cart");
        }

            public IActionResult Checkout()
        {

            return View();
        }
    }
}
