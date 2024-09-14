using System;
using System.Collections.Generic;

namespace DoAnCoSo.Models;

public partial class Category
{
    public int CatId { get; set; }

    public string? CatName { get; set; }

    public int? Ordering { get; set; }

    public int? ParentId { get; set; }

    public int? Levels { get; set; }

    public bool Published { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
