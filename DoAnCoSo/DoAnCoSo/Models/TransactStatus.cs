using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DoAnCoSo.Models;

public partial class TransactStatus
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StatusId { get; set; }

    public string? Status { get; set; }

    public string? DesStatus { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
