using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models;

public partial class Product 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProId { get; set; }

    public int? CatId { get; set; }

    public string ProName { get; set; } = null!;

    public string? ProImage { get; set; }

    public decimal ProPrice { get; set; }

    public int? Quantity { get; set; }

    public int? UnitlnStock { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public bool BestSellers { get; set; }

    public bool Active { get; set; }

    public bool HomeFlag { get; set; }

    public string? ShortDes { get; set; }

    public string? MetaDesc { get; set; }

    public string? MeetaKey { get; set; }

    public int? Rating { get; set; }

    public virtual Category? Cat { get; set; }
}
