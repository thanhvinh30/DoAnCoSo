using System;
using System.Collections.Generic;

namespace DoAnCoSo.Models;

public partial class TransactStatus
{
    public int StatusId { get; set; }

    public string? Status { get; set; }

    public string? DesStatus { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
