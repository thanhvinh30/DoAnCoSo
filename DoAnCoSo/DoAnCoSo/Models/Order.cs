using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DoAnCoSo.Models;

public partial class Order 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }

    public int? CusId { get; set; }

    public DateTime? OderDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public int? StatusId { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public string ShipType {  get; set; }

    public string? PaymentType { get; set; }

    public string? Note { get; set; }

    public int? Quantity { get; set; }
    public decimal? TotalMoney { get; set; }
    public string CusFullName {  get; set; }
    public string CusAddress { get; set; }
    public DateTime NeedDate { get; set; }
    public decimal PayShip {  get; set; }
    public string CusPhone { get; set; }


    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Customer? Cus { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual TransactStatus? Status { get; set; }
}
