using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSo.Models;

public partial class Customer : IdentityUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CusId { get; set; }
    [Required(ErrorMessage = " Nhập Username")]

    public string CusName { get; set; }
    [DataType(DataType.Password), Required(ErrorMessage ="Làm ơn nhập password")]
    public string CusPassword { get; set; }

    [Required(ErrorMessage = " Nhập Email"), EmailAddress]
    public string CusEmail { get; set; }

    public string Address { get; set; }

    public DateOnly Birthday { get; set; }

    public int Phone { get; set; }

    public int LocationId { get; set; }

    public DateOnly CreateDate { get; set; }

    public DateOnly LastLogin { get; set; }

    public string Avatar { get; set; }

    public bool Active { get; set; }

    public string Salt { get; set; }

    public virtual Location Location { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
