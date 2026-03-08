using System;
using System.Collections.Generic;

namespace DBconnector.Models;

public partial class GridPo
{
    public int GridPosId { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int? BinId { get; set; }

    public int GridId { get; set; }

    public virtual Bin? Bin { get; set; }

    public virtual Grid Grid { get; set; } = null!;
}
