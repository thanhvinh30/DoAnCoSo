using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class AppUserModel : IdentityUser
    {

            [Required]
            [StringLength(50, MinimumLength = 3)]
            public override string UserName { get; set; }

            [Required]
            [EmailAddress]
            public override string Email { get; set; }


    }
}
