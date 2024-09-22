using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DoAnCoSo.Models;

public partial class Contact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContactId { get; set; }

    public string? CusName { get; set; }

    public string? CusMess { get; set; }

    public string? CusEmail { get; set; }
}
