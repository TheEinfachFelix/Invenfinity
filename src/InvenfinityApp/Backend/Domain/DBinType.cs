using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBconnector.Models;

namespace Backend.Domain
{
    internal class DBinType
    {
        public DBinType(int BinTypeId, int SlotCount, int X, int Y)
        {
            this.BinTypeId = BinTypeId;
            this.SlotCount = SlotCount;
            this.X = X;
            this.Y = Y;
        }
        public int BinTypeId { get; }

        public int SlotCount { get; }

        public int X { get; }

        public int Y { get; }

        public virtual ICollection<DBin> Bins { get; } = new List<DBin>();
    }
}
