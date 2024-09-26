using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using DoAnCoSo.Respository;
using Microsoft.AspNetCore.Mvc.Rendering;
using DoAnCoSo.Models.ViewModels;
using NuGet.Protocol;
using DoAnCoSo.ModelView;
using DoAnCoSo.Extension;

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
            TempData["Success"] = "Bạn đã thêm sản phẩm thành công";
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
            TempData["Success"] = "Bạn đã Xóa sản phẩm thành công";
            return RedirectToAction("Cart");
        }
        public IActionResult CleanCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index","Home");
        }

        public IActionResult Checkout()
        {

            return View();
        }
        //-----------------------------------------------------------------------------


        public IActionResult Index()
        {
            List<int> lsproductsID = new List<int>();
            var lsGioHang = GioHang;
            
            return View(GioHang);
        }


        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh =new List<CartItem>();
                }
                return gh;
            }
        }

        [HttpPost]
        [Route("AddToCart.html")]
        public IActionResult AddToCart(int productID, int? amount)
        {
            List<CartItem> giohang = GioHang;

            try
            {
                CartItem item = GioHang.SingleOrDefault(p => p.Product.ProId == productID);
                if (item != null)
                {
                    if (amount.HasValue)
                    {
                        item.Amount = amount.Value;
                    }
                    else
                    {
                        item.Amount++;
                    }
                }
                else
                {
                    Product hh = _context.Products.SingleOrDefault(p => p.ProId == productID);
                    item = new CartItem
                    {
                        Amount = amount.HasValue ? amount.Value : 1,
                        Product = hh
                    };
                    giohang.Add(item);
                }
                HttpContext.Session.Set<List<CartItem>>("GioHang", giohang);
                return Json(new {success = true}); 
            }
            catch
            {
                return Json(new {success = false});
            }
        }


        [HttpPost]
        //[Route("emove.html")]
        public ActionResult remove(int productID)
        {
            try
            {
                List<CartItem> giohang = GioHang;
                CartItem item = giohang.SingleOrDefault(p => p.Product.ProId == productID);
                if (item != null)
                {
                    giohang.Remove(item);
                }
                HttpContext.Session.Set<List<CartItem>>("GioHang", giohang);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }



    }
}
