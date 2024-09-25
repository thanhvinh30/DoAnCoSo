using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DoAnCoSo.ModelView;                   // Mới Thêm
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DoAnCoSo.Extension;
using Microsoft.AspNetCore.Authentication;

namespace DoAnCoSo.Controllers
{
    public class HomeController : Controller
    {
        DataDoAnCoSoContext db = new DataDoAnCoSoContext();
        private readonly DataDoAnCoSoContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataDoAnCoSoContext context)
        {
            _logger = logger;
            _context = context;                         // Add
        }
        //[Authorize]
        public IActionResult MyAccount() 
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    var Lsorder = _context.Orders
                                                    .Include(x => x.Status)
                                                    .AsNoTracking()
                                                    .Where( x => x.CusId == khachhang.CusId)
                                                    .OrderByDescending( x => x.OderDate)
                                                    .ToList();
                    ViewBag.DonHang = Lsorder;
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login");
            //return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM model)
        {            
            try
            {

                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Customer");
                }
                if (ModelState.IsValid)
                {
                    var customerId = int.Parse(taikhoanID);
                    var taikhoan = _context.Customers.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null)
                    {
                        return RedirectToAction("Login", "Customer");
                    }
                    var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
                    if (pass != taikhoan.CusPassword)
                    {
                        ModelState.AddModelError("", "Mật khẩu hiện tại không chính xác.");
                        return View(model); // Trả về view với thông báo lỗi
                    }

                    // Kiểm tra mật khẩu mới và mật khẩu xác nhận có khớp nhau không
                    if (model.PasswordNew != model.ComfirmPasswordNew)
                    {
                        ModelState.AddModelError("", "Mật khẩu mới và mật khẩu xác nhận không khớp.");
                        return View(model); // Trả về view với thông báo lỗi
                    }

                    if (pass == taikhoan.CusPassword)
                    {
                        string passnew = (model.PasswordNew.Trim() + taikhoan.Salt.Trim()).ToMD5();
                        taikhoan.CusPassword = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        TempData["Success"] = "Thay đổi mật khẩu Thành Công";
                        return RedirectToAction("MyAccount", "Home");
                    }


                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thay đổi mật khẩu. Vui lòng thử lại.");
                return RedirectToAction("MyAccount", "Home");
            }
            TempData["Success"] = "Thay đổi mật khẩu  không thành công";
            return RedirectToAction("MyAccount", "Home");

        }


        [HttpPost]
        public IActionResult UpdateProfile(CustomerUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Trả về form với lỗi nếu dữ liệu không hợp lệ
            }

            var customerId = HttpContext.Session.GetString("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer"); // Chuyển về trang đăng nhập nếu chưa đăng nhập
            }

            var customer = _context.Customers.Find(int.Parse(customerId));
            if (customer == null)
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản.");
                return View(model);
            }

            // Cập nhật thông tin khách hàng
            customer.CusName = model.CusName;
            customer.Phone = model.Phone;
            customer.Address = model.Address;
            customer.LastLogin = DateOnly.FromDateTime(DateTime.Now);

            // Nếu người dùng không nhập ngày sinh, giữ nguyên giá trị hiện tại
            customer.Birthday = model.Birthday.HasValue ? DateOnly.FromDateTime(model.Birthday.Value) : customer.Birthday;


            // Lưu thay đổi
            _context.Update(customer);
            _context.SaveChanges();

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("MyAccount", "Home"); // Chuyển về trang hồ sơ sau khi cập nhật
        }



        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }



        public async Task<IActionResult> Index()
        {
            // Bắt đầu
            var categoriesWithProductCount = await _context.Categories
                .Select(c => new
                {
                    c.CatId,
                    c.CatName,
                    ProductCount = _context.Products.Count(p => p.CatId == c.CatId)
                }).ToListAsync();

            HomeViewVM vm = new HomeViewVM();

            var lsproducts = await _context.Products
                .AsNoTracking()
                .Where(x => x.Active == true && x.HomeFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();

            List<ProductHomeVM> lsProductsView = new List<ProductHomeVM>();

            var lsCats = await _context.Categories
                .AsNoTracking()
                .Where(x => x.Published == true && x.ParentId == 0)
                .OrderByDescending(x => x.Ordering)
                .ToListAsync();

            foreach (var item in lsCats)
            {
                ProductHomeVM productHomeVM = new ProductHomeVM();
                productHomeVM.lsCategory = item;
                productHomeVM.lsProducts = lsproducts
                    .Where(x => x.CatId == item.CatId)
                    .ToList();
                lsProductsView.Add(productHomeVM);
            }

            var bestSellerProducts = await _context.Products
                .AsNoTracking()
                .Where(p => p.Active == true && p.BestSellers == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(6)
                .ToListAsync();

            vm.Products = lsProductsView;
            vm.Categories = lsCats;

            ViewBag.AllProducts = lsproducts;
            ViewBag.lsCat = lsCats;
            ViewBag.BestSellerProducts = bestSellerProducts;

            return View(vm);
        }



        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Warranty_Policy()
        {
            return View();
        }
        public IActionResult Return_Policy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if(statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            
        }
    }
}
