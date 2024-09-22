using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DoAnCoSo.Models;

public partial class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CatId { get; set; }

    public string? CatName { get; set; }

    public int? Ordering { get; set; }

    public int? ParentId { get; set; }

    public int? Levels { get; set; }

    public bool Published { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
