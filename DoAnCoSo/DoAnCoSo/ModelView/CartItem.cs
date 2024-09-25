
using DoAnCoSo.Models;



using DoAnCoSo.Models;

namespace DoAnCoSo.ModelView
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Amount {  get; set; }
        public decimal TotalMoneyCart => Amount * Product.ProPrice;
    }
}
