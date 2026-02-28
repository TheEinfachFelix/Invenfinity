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
        public DBin(int BinID, DBinType binType)
        {
            this.BinId = BinID;
            this.BinType = binType;

            // Bei bintype registrieren
            binType.Bins.Add(this);

            Slots = new List<DPart?>();
            for (int i = 0; i < BinType.SlotCount; i++)
            {
                Slots.Add(null);
            }
        }

        public int BinId { get; }
        public List<DPart?> Slots { get; }
        public virtual DBinType BinType { get; }
        public virtual DGrid? Grid { get; set; }

        public BinPos? GetPos()
        {
            if (Grid == null) return null;
            return Grid.GetBinPosInGrid(this);
        }

        public void AddPart(DPart inPart, int SlotNr)
        {
            if (SlotNr < 0 || SlotNr >= BinType.SlotCount) throw new Exception("SlotNr out of range");
            if (Slots[SlotNr] != null) throw new Exception("Slot already filled");
            Slots[SlotNr] = inPart;
            inPart.Bins.Add(this);
        }

        public void RemovePart(DPart inPart)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i] != null && Slots[i]!.PartId == inPart.PartId)
                {
                    Slots[i] = null;
                    inPart.Bins.Remove(this);
                    return;
                }
            }
            throw new Exception("Part not found in bin");
        }
    }
}
