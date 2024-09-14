using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;
using Azure;
using PagedList.Core;
using DoAnCoSo.Helpper;
using NuGet.Packaging.Signing;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SearchController : Controller
    {
        private readonly DataDoAnCoSoContext _context;

        public SearchController(DataDoAnCoSoContext context)
        {
            _context = context;
        }
        [HttpPost]
        //public IActionResult FindProducts(string keyword)
        //{
        //    List<Product> ls = new List<Product>();
        //    if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
        //    {
        //        return PartialView("SearchProducts_PartialView", null);
        //    }
        //    ls = _context.Products
        //                            .AsNoTracking()
        //                            .Include(s => s.CatId)
        //                            .Where( x => x.ProName.Contains(keyword))
        //                            .OrderByDescending(x => x.ProName)
        //                            .ToList();
        //    if ( ls == null)
        //    {
        //        return PartialView("SearchProducts_PartialView", null);
        //    }
        //    else 
        //    {
        //        return PartialView("SearchProducts_PartialView", ls);
        //    }
        //}
        public IActionResult FindProducts(string keyword)
        {
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("SearchProducts_PartialView", new List<Product>());
            }

            var products = _context.Products
                                   .AsNoTracking()
                                   .Include(p => p.Cat) // Include the Category instead of CatId
                                   .Where(p => p.ProName.Contains(keyword))
                                   .OrderByDescending(p => p.ProName)
                                   .ToList();

            return PartialView("SearchProducts_PartialView", products);
        }

    }
}
