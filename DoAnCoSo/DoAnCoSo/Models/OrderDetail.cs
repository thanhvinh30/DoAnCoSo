using System;
using System.Collections.Generic;

namespace DoAnCoSo.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProId { get; set; }

    public string? ProName { get; set; }

    public string? ProImage { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? PaymentType { get; set; }

    public string? StatusOrderDetail { get; set; }

    public double? Price { get; set; }

    public int? Quantity { get; set; }

    public double? Total { get; set; }

    public virtual Order? Order { get; set; }
}
