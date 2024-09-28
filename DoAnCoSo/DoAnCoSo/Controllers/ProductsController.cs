//using Azure;
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


            //Start
            // Tính toán số lượng sản phẩm theo từng danh mục
            var categoriesWithProductCount = _context.Categories
                .Where( c => c.Published)
            .Select(c => new
            {
                c.CatId,
                c.CatName,
                ProductCount = _context.Products.Count(p => p.CatId == c.CatId && p.Active)
            }).ToList();
            // Truyền thêm thông tin số lượng sản phẩm của mỗi danh mục qua View
            ViewBag.CategoriesWithProductCount = categoriesWithProductCount;
            //End


            if (CatID != 0)
            {
                lsProducts = _context.Products
                        .AsNoTracking()
                        .Where(p => p.CatId == CatID && p.ProId >= 1 && p.Cat.Published && p.Active)
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
                        .Where(c => c.Cat.Published && c.Active)
                        .OrderByDescending(x => x.ProId)
                        .OrderBy(x => x.ProId)
                        .ToList();
            }


            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);           // Fix lỗi về lsProducts 
            //PagedList<Product> models = new PagedList<Product>(lsProducts.AsEnumerable(), pageNumber, pageSize);

            ViewBag.CurrentCateId = CatID;
            ViewBag.Currentpage = pageNumber;


            ViewData["DanhMuc"] = new SelectList(_context.Categories.Where(c => c.Published), "CatId", "CatName", CatID);
            ViewBag.CurrentPage = pageNumber;
            return View(models);

        }
        [Route("List/{id}.html", Name = "ListProducts")]
        public IActionResult List(string ProName, int page = 1)
        {
                try
                {
                    //var pageNumber = page == null page <= 0 ? 1 : page.Value;
                    var pageSize = 6;
                    var Category = _context.Categories.AsNoTracking().SingleOrDefault(x => x.CatName == ProName);
                    var lsCate = _context.Products  .AsNoTracking()
                                                    .Where( x => x.CatId == Category.CatId)
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
        public IActionResult Details(int id,int CatID = 0)
        {
            try
            {
                //Start
                // Tính toán số lượng sản phẩm theo từng danh mục
                var categoriesWithProductCount = _context.Categories
                    .Where(c => c.Published)
                .Select(c => new
                {
                    c.CatId,
                    c.CatName,
                    ProductCount = _context.Products.Count(p => p.CatId == c.CatId && p.Active)
                }).ToList();
                // Truyền thêm thông tin số lượng sản phẩm của mỗi danh mục qua View
                ViewBag.CategoriesWithProductCount = categoriesWithProductCount;
                //End
                var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProId == id);

                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                
                var lsproducts = _context.Products
                                                    .AsNoTracking()
                                                    .Where(x => x.CatId == product.CatId && x.ProId != id && x.Active == true)
                                                    .Take(5)    
                                                    .OrderBy( x => x.DateCreated)
                                                    .ToList();
                ViewData["DanhMuc"] = new SelectList(_context.Categories.Where(c => c.Published), "CatId", "CatName", CatID);
                ViewBag.CurrentProductsList = lsproducts;
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
    }
}
