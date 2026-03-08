using System;
using System.Collections.Generic;

namespace DBconnector.Models;

public partial class Bin
{
    public int BinId { get; set; }

    public int BinTypeId { get; set; }

    public virtual ICollection<BinSlot> BinSlots { get; set; } = new List<BinSlot>();

    public virtual BinType BinType { get; set; } = null!;

    public virtual ICollection<GridPo> GridPos { get; set; } = new List<GridPo>();
}
