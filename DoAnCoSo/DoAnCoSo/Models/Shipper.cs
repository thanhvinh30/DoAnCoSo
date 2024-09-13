using System;
using System.Collections.Generic;

namespace DoAnCoSo.Models;

public partial class Shipper
{
    public int ShipperId { get; set; }

    public string ShipperName { get; set; }

    public string Phone { get; set; }

    public string Company { get; set; }

    public DateOnly ShipDate { get; set; }
}
