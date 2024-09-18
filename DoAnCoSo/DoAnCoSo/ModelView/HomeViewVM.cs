using System.Collections.Generic;
using DoAnCoSo.Models;

namespace DoAnCoSo.ModelView
{
    public class HomeViewVM
    {
        public List<ProductHomeVM> Products { get; set; }
        public List<Category> Categories { get; set; }  // Add this line to hold categories
    }
}
