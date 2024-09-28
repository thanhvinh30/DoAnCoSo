using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using DoAnCoSo.Helpper;
using Microsoft.AspNetCore.Authorization;
using DoAnCoSo.Extension;
using DoAnCoSo.ModelView;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using DoAnCoSo.Respository;

namespace DoAnCoSo.Controllers
{
    //[Authorize]
    public class CustomerController : Controller
    {
        #region  Private
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _siginManager;
        private readonly DataDoAnCoSoContext _context;
        public CustomerController(DataDoAnCoSoContext context, UserManager<AppUserModel> userManager, SignInManager<AppUserModel> siginManager)
        {
            _userManager = userManager;
            _siginManager = siginManager;
            _context = context;

        }
        #endregion


        public List<Cart> cartItems => HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();

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
        //[AllowAnonymous]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("MyAccount", "Customer");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {                                             
                return RedirectToAction("MyAccount", "Customer");
            }
            return View();
        }
        [HttpPost]
        //[AllowAnonymous]
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



                    //ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Login");
                    //claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);



                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true // Để giữ trạng thái đăng nhập qua các phiên làm việc
                    };

                    //await HttpContext.SignInAsync(claimsPrincipal);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
                    TempData["Success"] = "Đăng Nhập Thành Công";
                    // Kiểm tra xem returnUrl có giá trị hay không, nếu có thì chuyển hướng về trang đó
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

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
        //[AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        //[AllowAnonymous]
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
        #endregion

        #region  MyAccount
        // Nếu yêu cầu quyền Admin
        [Authorize]
        [AllowAnonymous]
        public IActionResult MyAccount()
        {
            //var taikhoanID = HttpContext.Session.GetString("CustomerId");
            //var cusIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CusId");
            //if (taikhoanID != null)
            //{
            //    var cusId = Convert.ToInt32(cusIdClaim.Value);
            //    var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == cusId);
            //    if (khachhang != null)
            //    {
            //        var Lsorder = _context.Orders
            //                                    .Include(x => x.Status)
            //                                    .AsNoTracking()
            //                                    .Where(x => x.CusId == khachhang.CusId)
            //                                    .OrderByDescending(x => x.OderDate)
            //                                    .ToList();
            //        ViewBag.DonHang = Lsorder;
            //        return View(khachhang);
            //    }
            //}
            //return RedirectToAction("Login");



            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    var Lsorder = _context.Orders
                                                    .Include(x => x.Status)
                                                    .AsNoTracking()
                                                    .Where(x => x.CusId == khachhang.CusId)
                                                    .OrderByDescending(x => x.OrderDate)
                                                    .ToList();
                    ViewBag.LastLogout = khachhang.LastLogin;
                    ViewBag.DonHang = Lsorder;
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login");
        }
        #endregion

        #region  System
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
                        return RedirectToAction("MyAccount", "Customer");
                    }


                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thay đổi mật khẩu. Vui lòng thử lại.");
                return RedirectToAction("MyAccount", "Customer");
            }
            TempData["Success"] = "Thay đổi mật khẩu  không thành công";
            return RedirectToAction("MyAccount", "Customer");

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
            return RedirectToAction("MyAccount", "Customer"); // Chuyển về trang hồ sơ sau khi cập nhật
        }
        [HttpGet]
        public IActionResult Logout()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            //var customer = _context.Customers.Find(int.Parse(customerId));
            if (customerId != null)
            {
                // Lấy khách hàng từ cơ sở dữ liệu (đồng bộ)
                var khachhang = _context.Customers.Find(Convert.ToInt32(customerId));
                if (khachhang != null)
                {
                    // Cập nhật thời gian đăng xuất
                    khachhang.LastLogin = DateOnly.FromDateTime(DateTime.Now);
                    _context.Update(khachhang);
                    _context.SaveChanges();  // Sử dụng phương thức đồng bộ để lưu thay đổi
                }
            }
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");

            
            return RedirectToAction("Index", "Home");
        }
        #endregion




        public IActionResult Checkout()
        {
            if (cartItems.Count == 0)
            {
                return RedirectToAction("Login", "Customer");
            }
            return View(cartItems);

            //var customerId = HttpContext.Session.GetString("CustomerId");
            //if (customerId == null && cartItems.Count == 0)
            //{
            //    // Chưa đăng nhập, chuyển hướng đến trang đăng nhập với returnUrl là Checkout
            //    return RedirectToAction("Login", "Customer", new { returnUrl = Url.Action("Checkout", "Customer") });
            //}

            //// Xử lý tiếp nếu đã đăng nhập
            //return View(cartItems);
        }
    }
}
