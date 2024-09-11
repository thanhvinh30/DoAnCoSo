using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;
using PagedList.Core;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly DataDoAnCoSoContext _context;
        //Chức Năng thông báo Start
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            switch (type)
            {
                case "Success":
                    TempData["AlertType"] = "alert-Success"; break;
                case "Warning":
                    TempData["AlertType"] = "alert-Warning"; break;
                case "Error":
                    TempData["AlertType"] = "alert-Error"; break;
                default: TempData["AlertType"] = ""; break;
            }
        }

        //Chức Năng thông báo End
        // Thêm mới Filtter

        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Admin/AdminProducts?CatID = {CatID}";
            if (CatID != 0)
            {
                url = $"Admin/AdminProducts";
            }
            return Json(new { status = "Success", redirectUrl = url });
        }
        public AdminProductsController(DataDoAnCoSoContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminProducts
        public async Task<IActionResult> Index(int page =1, int CatID = 0)
        {
            //var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //var pageNumber = page;
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

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProId,CatId,ProName,ProImage,ProPrice,Quantity,UnitlnStock,DateCreated,DateModified,BestSellers,Active,HomeFlag,ShortDes,MetaDesc,MeetaKey")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProId,CatId,ProName,ProImage,ProPrice,Quantity,UnitlnStock,DateCreated,DateModified,BestSellers,Active,HomeFlag,ShortDes,MetaDesc,MeetaKey")] Product product)
        {
            if (id != product.ProId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    SetAlert("Đã sửa thành công", "Success");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProId))
                    {
                        SetAlert("Sửa sai rầu", "Error");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProId == id);
        }
    }
}
