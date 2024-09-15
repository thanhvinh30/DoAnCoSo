using Azure;
using DoAnCoSo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace DoAnCoSo.Controllers
{
    public class ProductsController : Controller
    {
        DataDoAnCoSoContext db = new DataDoAnCoSoContext();
        private readonly DataDoAnCoSoContext _context;

        public ProductsController(DataDoAnCoSoContext context)
        {
            _context = context;
        }
        [Route("/shop.html", Name = "Shop-Products")]
        public IActionResult Products(int? page)
        {


            //int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //int pageSize = 6;
            //var lsproducts = db.Products
            //    .AsNoTracking()
            //    .OrderBy(x => x.ProId)
            //    .ToList();
            //// .ToPagedList(pageNumber, pageSize);

            //PagedList<Product> lstpro = new PagedList<Product>(lsproducts.AsQueryable(), pageSize, pageNumber);
            //ViewBag.CurrentPage = pageNumber; // Để giữ giá trị phân loại trong view
            //return View(lsproducts);
            // Dùng try catch, nếu có lỗi thì dễ xử lí hơn
            try
            {
            int pageNumber = page  == null || page <= 0 ? 1 : page.Value;
            int pageSize = 6;
            var lsproducts = db.Products
                .AsNoTracking()
                .OrderByDescending(x => x.DateCreated);
                //.ToList();
               // .ToPagedList(pageNumber, pageSize);

            PagedList<Product> models = new PagedList<Product>(lsproducts, pageSize, pageNumber);
            ViewBag.CurrentPage = pageNumber; // Để giữ giá trị phân loại trong view
            return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
          
        }
        public IActionResult Cart()
        {
            return View();
        }
        [Route("/{Alias}-{id}", Name = "ListProducts")]
        public IActionResult List(int id, int page = 1)
        {
                try
                {
                    //var pageNumber = page == null page <= 0 ? 1 : page.Value;
                    var pageSize = 6;
                    var Category = _context.Categories.Find(id);
                    var lsCate = _context.Products  .AsNoTracking()
                                                    .Where( x => x.CatId == id)
                                                    .OrderByDescending(x => x.DateCreated);


                    PagedList<Product> models = new PagedList<Product>(lsCate, pageSize, page);
                    ViewBag.CurrentPage = page; // Để giữ giá trị phân loại trong view
                    ViewBag.CurrentCate = Category;
                    return View(models);
                }
                catch
                {
                     return RedirectToAction("Index", "Home");
                }
           

        }
        [Route("/{Alias}-{id}.html", Name ="ProductsDetail")]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProId == id);

                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                return View();

            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            //var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProId == id);

            //if (product == null)
            //{
            //    return RedirectToAction("Index");
            //}

            //return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
