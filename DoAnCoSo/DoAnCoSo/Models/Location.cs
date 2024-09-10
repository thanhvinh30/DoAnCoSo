using System;
using System.Collections.Generic;

namespace DoAnCoSo.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string? Name { get; set; }

    public string? NameWithType { get; set; }

    public string? PathWithType { get; set; }
}
