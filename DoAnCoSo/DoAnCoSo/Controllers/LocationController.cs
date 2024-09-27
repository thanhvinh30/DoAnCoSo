


using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSo.Controllers
{
    public class LocationController : Controller
    {
        DataDoAnCoSoContext db = new DataDoAnCoSoContext();
        private readonly DataDoAnCoSoContext _context;

        public LocationController(DataDoAnCoSoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public ActionResult QuanHuyenList( int LocationID)
        {
            var QuanHuyens = _context.Locations.OrderBy( x => x.LocationId)
                                               .Where( x => x.ParentCode == LocationID && x.Level == 2)
                                               .OrderBy( x => x.Name)
                                               .ToList();
            return Json(QuanHuyens);
        }

        public ActionResult PHuongXaList(int LocationID)
        {
            var PhuongXas = _context.Locations.OrderBy(x => x.LocationId)
                                               .Where(x => x.ParentCode == LocationID && x.Level == 2)
                                               .OrderBy(x => x.Name)
                                               .ToList();
            return Json(PhuongXas);
        }

    }
}
