using System;
using System.Collections.Generic;

namespace DoAnCoSo.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? UserNameAcc { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateOnly? LastLogin { get; set; }

    public int? RoleId { get; set; }

    public string? PasswordAcc { get; set; }

    public bool Active { get; set; }

    public DateOnly? CreateDate { get; set; }

    public string? Salt { get; set; }

    public virtual Role? Role { get; set; }
}
