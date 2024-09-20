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

            // Moiwsi --------------------------------------------------------------
            // Lấy sản phẩm từ database
            //var product = await _context.Products.FindAsync(id);
            //if (product == null)
            //{
            //    return NotFound(); // Kiểm tra nếu không tìm thấy sản phẩm
            //}

            //// Lấy giỏ hàng từ session hoặc tạo mới nếu chưa có
            //List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();

            //// Tìm sản phẩm trong giỏ hàng
            //var cartItem = cart.FirstOrDefault(c => c.ProId == id);

            //if (cartItem == null)
            //{
            //    // Nếu chưa có trong giỏ, thêm mới
            //    cart.Add(new Cart(product));
            //}
            //else
            //{
            //    // Nếu đã có, tăng số lượng
            //    cartItem.Quantity++;
            //}

            //// Cập nhật lại giỏ hàng vào session
            //HttpContext.Session.SetJson("Cart", cart);

            //// Chuyển hướng về trang trước đó
            //return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult Checkout()
        {

            return View();
        }
    }
}
