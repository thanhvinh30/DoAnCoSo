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

namespace DoAnCoSo.Controllers
{
    public class AccountController : Controller
    {
        DataDoAnCoSoContext db = new DataDoAnCoSoContext();
        private readonly DataDoAnCoSoContext _context;

        public AccountController(DataDoAnCoSoContext context)
        {
            _context = context;
        }
        public IActionResult Account()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name ="Đăng Ký")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "Đăng Ký")]
        public async Task<IActionResult> DangKyTaiKhoan(RegisterVM taikhoan)
        {
            try
            {
                if( ModelState.IsValid)
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
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        // Lưu session Makh
                        HttpContext.Session.SetString("CusId", khachhang.CusId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CusId");
                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.CusName),
                            new Claim("CusId", khachhang.CusId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);

                        return RedirectToAction("Dashboard","Account");
                    }
                    catch
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Account");
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
