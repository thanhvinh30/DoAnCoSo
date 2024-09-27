using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DoAnCoSo.Models;

public partial class Location 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LocationId { get; set; }

    public string? Name { get; set; }

    public string? NameWithType { get; set; }

    public string? PathWithType { get; set; }
    public int? Level { get; set; }
    public int? ParentCode { get; set; }
    public string? Type { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
