using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;
using PagedList.Core;
using Microsoft.AspNetCore.Authorization;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminCustomersController : Controller
    {
        private readonly DataDoAnCoSoContext _context;

        public AdminCustomersController(DataDoAnCoSoContext context)
        {
            _context = context;
        }

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

        // GET: Admin/AdminCustomers
        //public async Task<IActionResult> Index(int? page)
        //{
        //    var pageNumber = page == null || page <= 0 ? 1 : page.Value;                                    // tùy theo số lượng biến
        //    var pageSize = 10;                                                                              // Số lượng biến trong page
        //    var lsCustomers = _context.Customers
        //                            .AsNoTracking()
        //                            .Include(x => x.Location)
        //                            .OrderByDescending(x => x.CreateDate);

        //    PagedList<Customer> models = new PagedList<Customer>(lsCustomers, pageNumber, pageSize);
        //    ViewBag.CurrentPage = pageNumber;
        //    return View(models);
        //    //return View(await _context.Customers.ToListAsync());
        //}
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;

            var lsCustomers = await _context.Customers
                                           .AsNoTracking()
                                           .Include(x => x.Location)
                                           .OrderByDescending(x => x.CreateDate)
                                           .ToListAsync(); // Chú ý sử dụng ToListAsync để lấy dữ liệu

            if (lsCustomers == null || !lsCustomers.Any())
            {
                // Kiểm tra nếu không có khách hàng nào
                ViewBag.Message = "Không có dữ liệu khách hàng";
            }

            PagedList<Customer> models = new PagedList<Customer>(lsCustomers.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }




        // GET: Admin/AdminCustomers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CusId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admin/AdminCustomers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCustomers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CusId,CusName,CusPassword,CusEmail,Address,Birthday,Phone,LocationId,CreateDate,LastLogin,Avatar,Active")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Admin/AdminCustomers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Admin/AdminCustomers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CusId,CusName,CusPassword,CusEmail,Address,Birthday,Phone,LocationId,CreateDate,LastLogin,Avatar,Active")] Customer customer)
        {
            if (id != customer.CusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CusId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Admin/AdminCustomers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CusId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admin/AdminCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CusId == id);
        }
    }
}
