using DoAnCoSo.Extension;
using DoAnCoSo.Models;
using DoAnCoSo.ModelView;
using DoAnCoSo.Respository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Controllers
{
    public class CheckOutController : Controller
    {
        DataDoAnCoSoContext db = new DataDoAnCoSoContext();
        private readonly DataDoAnCoSoContext _context;

        public CheckOutController(DataDoAnCoSoContext context)
        {
            _context = context;
        }


        public List<Cart> GioHang
        {
            get
            {
                List<Cart> gh = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
                if (gh == default(List<Cart>))
                {
                    gh = new List<Cart>();
                }
                return gh;
            }
        }


        public IActionResult Index(string returnUrl = null)
        {
            List<Cart> gh = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking()
                                                    .SingleOrDefault(x => x.CusId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CusId;
                model.FullName  = khachhang.CusName;
                model.Email = khachhang.CusEmail;
                model.PhoneNumber = khachhang.Phone;
                model.Address = khachhang.Address;


            }
            ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Level == 1).OrderBy(x => x.Type).ToList(), "LocationID");
            ViewBag.GioHang = gh;

            
            return View(model);
        }
    }
}
