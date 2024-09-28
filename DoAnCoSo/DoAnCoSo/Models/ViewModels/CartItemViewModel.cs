using DoAnCoSo.ModelView;

namespace DoAnCoSo.Models.ViewModels
{
    public class CartItemViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal GrandToTal {  get; set; }
        //public List<Product> ProItems { get; set; }
        public List<CartItem> items { get; set; }
        public List<Product> products { get; set; }
        public List<Customer> customers { get; set; }
    }
}
