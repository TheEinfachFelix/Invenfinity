using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Backend.Domain
{
    internal class DBin
    {
        public DBin(int BinID, List<DPart?> Slots, DBinType binType)
        {
            this.BinId = BinID;
            this.Slots = Slots;
            this.BinType = binType;

            // bei Parts registrieren
            foreach (var item in Slots)
            {
                if (item == null) continue;
                item.Bins.Add(this);
            }

            // Bei bintype registrieren
            binType.Bins.Add(this);
        }

        public int BinId { get; }
        public List<DPart?> Slots { get; }
        public virtual DBinType BinType { get; set; } = null!;
        public virtual DGrid? Grid { get; set; }

        public BinPos? GetPos()
        {
            if (Grid == null) return null;
            return Grid.GetBinPosInGrid(this);
        }

    }
}
