using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Backend.Domain
{
    internal class DBin
    {
        public DBin(int BinID, List<DPart> Slots, DBinType binType, DGrid Grid)
        {
            this.BinId = BinID;
            this.Slots = Slots;
            this.BinType = binType;
            this.Grid = Grid;

            foreach (var item in Slots)
            {
                item.Bins.Add(this);
            }
            binType.Bins.Add(this);
        }
        public int BinId { get; }


        public List<DPart> Slots { get; }

        public virtual DBinType BinType { get; set; } = null!;
        public virtual DGrid Grid { get; set; }
    }
}
