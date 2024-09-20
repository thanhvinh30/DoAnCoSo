using System;
using System.Collections.Generic;

namespace DoAnCoSo.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int ProId { get; set; }

    public string ProName { get; set; }

    public string ProImage { get; set; }


    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal Total 
    { 
        get
        {
            return Quantity * Price;
        }
    }

    public virtual Order Order { get; set; }
    public Cart()
    {

    }
    public Cart(Product pro)
    {
        ProId = pro.ProId;
        ProName = pro.ProName;
        ProImage = pro.ProImage;
        Price = pro.ProPrice;
        Quantity = 1;
    }
}
