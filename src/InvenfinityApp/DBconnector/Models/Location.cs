using System;
using System.Collections.Generic;

namespace DBconnector.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string? Name { get; set; }

    public int? MasterLocationId { get; set; }

    public virtual ICollection<Grid> Grids { get; set; } = new List<Grid>();

    public virtual ICollection<Location> InverseMasterLocation { get; set; } = new List<Location>();

    public virtual Location? MasterLocation { get; set; }
}
