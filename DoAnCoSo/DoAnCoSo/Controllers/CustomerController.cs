using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using DoAnCoSo.Helpper;
using Microsoft.AspNetCore.Authorization;
using DoAnCoSo.Extension;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using DoAnCoSo.ModelView;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DoAnCoSo.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _siginManager;

        private readonly DataDoAnCoSoContext _context;

        public CustomerController(DataDoAnCoSoContext context, UserManager<AppUserModel> userManager, SignInManager<AppUserModel> siginManager)
        {
            _userManager = userManager;
            _siginManager = siginManager;
            _context = context;

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện Thoại: " + Phone + " đã được sử dụng");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email: " + Email + " đã được sử dụng");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        public IActionResult MyAccount()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                return RedirectToAction("MyAccount", "Customer");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel customer, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);

                    var khachhang = _context.Customers.AsNoTracking()
    .SingleOrDefault(x => x.CusEmail.Trim() == customer.UserName);

                    // Kiểm tra khachhang có null hay không
                    if (khachhang == null)
                    {
                        ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không hợp lệ.");
                        return RedirectToAction("Register", "Customer");
                    }
                    // So sánh mật khẩu
                    string pass = (customer.Password + khachhang.Salt.Trim()).ToMD5();
                    if (khachhang.CusPassword != pass)
                    {
                        ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không hợp lệ.");
                        return View(customer); // Trả về form đăng nhập với lỗi
                    }
                    if (khachhang.Active == false) return RedirectToAction("Index", "Home");

                    HttpContext.Session.SetString("CustomerId", khachhang.CusId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.CusName),
                        new Claim("CusId", khachhang.CusId.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);
                    TempData["Success"] = "Đăng Nhập Thành Công";
                    return RedirectToAction("MyAccount", "Home");
                }

            }
            //catch 
            //{
            //    // Ghi lại lỗi chi tiết
            //    RedirectToAction("Register", "Customer");
            //}
            catch// Bắt tất cả các ngoại lệ khác
            {
                RedirectToAction("Register", "Customer");
            }
            return RedirectToAction("Register", "Customer"); //return //RedirectToAction("Register", "Customer");
        } 



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {



                    string salt = Utilities.GetRanDomKey();
                    Customer khachhang = new Customer
                    {
                        CusName = taikhoan.FulllName,
                        Phone = taikhoan.Phone.Trim().ToLower(),
                        CusEmail = taikhoan.Email.Trim().ToLower(),
                        CusPassword = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Salt = salt,
                        Active = true,
                        CreateDate = DateOnly.FromDateTime(DateTime.Now)
                    };

                    //
                    //
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        // Lưu session Makh
                        HttpContext.Session.SetString("CustomerId", khachhang.CusId.ToString());
                        //
                        //
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        //Identity

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.CusName),
                            new Claim("CustomerId", khachhang.CusId.ToString())
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        TempData["Success"] = "Đăng Nhập Thành Công";
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("MyAccount", "Customer");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Register", "Customer");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }

    }
}
