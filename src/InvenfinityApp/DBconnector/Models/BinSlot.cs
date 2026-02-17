using System;
using System.Collections.Generic;

namespace DBconnector.Models;

public partial class BinSlot
{
    public int BinId { get; set; }

    public int PartId { get; set; }

    public int SlotNr { get; set; }

    public virtual Bin Bin { get; set; } = null!;

    public virtual Part Part { get; set; } = null!;
}
