using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models;

public partial class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? DesRole { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
