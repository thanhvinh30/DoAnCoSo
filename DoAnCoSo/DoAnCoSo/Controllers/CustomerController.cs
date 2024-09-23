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
        public IActionResult Account()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanID));
                if (khachhang == null) return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("Index");
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel customer, string url)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);

                    var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusEmail.Trim() == customer.UserName);
                    if (khachhang == null) return RedirectToAction("Register","Customer");

                    string pass = (customer.Password + khachhang.Salt.Trim().ToMD5());
                    if (customer.Password != pass)
                    {
                        return View(customer);
                    }
                    if (khachhang.Active == false)
                    {
                        return RedirectToAction("","Customer");
                    }
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
                    return RedirectToAction("Checkout","Products");
                }
            }
            catch
            {
                return RedirectToAction("Register","Customer");
            }
            return View(customer);
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

                        TempData["Warning"] = "Error Roàiii";
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Index", "Home");
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
