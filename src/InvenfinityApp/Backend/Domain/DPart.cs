using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBconnector.Models;

namespace Backend.Domain
{
    internal class DPart
    {
        public DPart(int PartID, int? InventreeID)
        {
            this.PartId = PartID;
            this.InventreeId = InventreeID;
        }
        public int PartId { get; set; }

        public int? InventreeId { get; set; }

        public virtual ICollection<DBin> Bins { get; } = new List<DBin>();
    }
}
