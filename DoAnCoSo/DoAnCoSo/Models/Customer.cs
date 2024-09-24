using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSo.Models;

public class Customer
{

    public int CusId { get; set; }
    [Required(ErrorMessage = " Nhập họ và tên")]

    public string CusName { get; set; }
    [DataType(DataType.Password), Required(ErrorMessage ="Làm ơn nhập password")]
    public string CusPassword { get; set; }

    [Required(ErrorMessage = " Nhập Email"), EmailAddress]

    public string CusEmail { get; set; }

    public string? Address { get; set; }

    public DateOnly Birthday { get; set; }

    public string Phone { get; set; }

    public int LocationId { get; set; }

    public DateOnly CreateDate { get; set; }

    public DateOnly LastLogin { get; set; }


    public bool Active { get; set; }

    public string Salt { get; set; }

    public virtual Location Location { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
