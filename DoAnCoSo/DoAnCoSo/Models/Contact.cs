using System;
using System.Collections.Generic;

namespace DoAnCoSo.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public string? CusName { get; set; }

    public string? CusMess { get; set; }

    public string? CusEmail { get; set; }
}
