using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class BinType
{
    public int BinTypeId { get; set; }

    public int SlotCount { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();
}
