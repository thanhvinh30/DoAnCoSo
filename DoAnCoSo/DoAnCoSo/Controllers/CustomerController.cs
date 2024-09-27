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
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DoAnCoSo.Controllers
{

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

        #region  Validate
        [HttpGet]
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
        #endregion

        #region  Login
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {                                             
                return RedirectToAction("MyAccount", "Customer");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel customer, string? returnUrl )
        {
            ViewBag.ReturnUrl = returnUrl;
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
                    else
                    {
                        if(!khachhang.Active)
                        {
                            ModelState.AddModelError("Lỗi", "Tài khoản đã bị khóa");
                        }
                        else
                        {
                            // So sánh mật khẩu
                            string pass = (customer.Password + khachhang.Salt.Trim()).ToMD5();
                            if (khachhang.CusPassword != pass)
                            {
                                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không hợp lệ.");
                                return View(customer); // Trả về form đăng nhập với lỗi
                            }
                            //if (khachhang.Active == false) return RedirectToAction("Index", "Home");
                        }
                    }                   
                  
                    HttpContext.Session.SetString("CustomerId", khachhang.CusId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.CusName),
                        new Claim("CusId", khachhang.CusId.ToString()),
                        new Claim(ClaimTypes.Role, "CustomerId")
                    };



                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Login");
                    claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true // Để giữ trạng thái đăng nhập qua các phiên làm việc
                    };

                    //await HttpContext.SignInAsync(claimsPrincipal);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    TempData["Success"] = "Đăng Nhập Thành Công";
                    return RedirectToAction("MyAccount", "Customer");
                }

            }
            //catch 
            //{
            //    // Ghi lại lỗi chi tiết
            //    RedirectToAction("Register", "Customer");
            //}
            catch// Bắt tất cả các ngoại lệ khác
            {
                return RedirectToAction("Register", "Customer");
            }
           
            return RedirectToAction("Register", "Customer"); //return //RedirectToAction("Register", "Customer");
        }
        #endregion

        #region Register
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
                        return RedirectToAction("MyAccount", "Home");
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
        #endregion



        [Authorize]
        public IActionResult MyAccount()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            var cusIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CusId");
            if (taikhoanID != null)
            {
                var cusId = Convert.ToInt32(cusIdClaim.Value);
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == cusId);
                if (khachhang != null)
                {
                    var Lsorder = _context.Orders
                                                .Include(x => x.Status)
                                                .AsNoTracking()
                                                .Where(x => x.CusId == khachhang.CusId)
                                                .OrderByDescending(x => x.OderDate)
                                                .ToList();
                    ViewBag.DonHang = Lsorder;
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login");
        }

    }
}
