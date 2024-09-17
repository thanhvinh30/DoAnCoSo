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
        //[Route("/shop.html", Name = "Shop-Products")]
        public IActionResult Products(int page = 1, int CatID = 0)
        {
           
            var pageNumber = page < 1 ? 1 : page;
            var pageSize = 6;
            List<Product> lsProducts = new List<Product>();
            if (CatID != 0)
            {
                lsProducts = _context.Products
                        .AsNoTracking()
                        .Where(p => p.CatId == CatID && p.ProId >= 1)
                        .Include(x => x.Cat)
                        .OrderByDescending(x => x.ProId)
                        .OrderBy(x => x.ProId)
                        .ToList();
            }
            else
            {

                lsProducts = _context.Products
                        .AsNoTracking()
                        .Include(x => x.Cat)
                        .OrderByDescending(x => x.ProId)
                        .OrderBy(x => x.ProId)
                        .ToList();
            }


            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);           // Fix lỗi về lsProducts                                                                                                                              //PagedList<Product> models = new PagedList<Product>(lsProducts.AsEnumerable(), pageNumber, pageSize);

            ViewBag.CurrentCateId = CatID;
            ViewBag.Currentpage = pageNumber;


            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", CatID);
            ViewBag.CurrentPage = pageNumber;
            return View(models);

        }
        public IActionResult Cart()
        {
            return View();
        }
        [Route("CatID.html", Name = "ListProducts")]
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
        [Route("Detail/{id}.html", Name = "ProductsDetail")]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProId == id);

                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                return View(product);

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
