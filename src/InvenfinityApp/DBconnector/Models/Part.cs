using System;
using System.Collections.Generic;

namespace DBconnector.Models;

public partial class Part
{
    public int PartId { get; set; }

    public int? InventreeId { get; set; }

    public virtual ICollection<BinSlot> BinSlots { get; set; } = new List<BinSlot>();
}
