using System;
using System.Collections.Generic;

namespace DoAnCoSo.Models;

public partial class Customer
{
    public int CusId { get; set; }

    public string? CusName { get; set; }

    public string? CusPassword { get; set; }

    public string? CusEmail { get; set; }

    public string? Address { get; set; }

    public DateOnly? Birthday { get; set; }

    public string Phone { get; set; }

    public int LocationId { get; set; }

    public DateOnly? CreateDate { get; set; }

    public DateOnly? LastLogin { get; set; }

    public string? Avatar { get; set; }

    public bool Active { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
