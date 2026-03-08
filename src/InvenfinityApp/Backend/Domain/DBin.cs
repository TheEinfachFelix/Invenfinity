using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Backend.Domain
{
    internal class DBin
    {
        public DBin(int BinID)
        {
            this.BinId = BinID;

            Slots = new List<DPart?>();
        }
        public DBin(int BinID, DBinType type)
        {
            this.BinId = BinID;

            Slots = new List<DPart?>();
            SetBinType(type);
        }


        public int BinId { get; }
        public List<DPart?> Slots { get; }
        public virtual DBinType BinType { get; private set; }
        public virtual DGrid? Grid { get; set; }
        private bool binSet = false;
        public void SetBinType(DBinType BinType)
        {
            if (binSet) throw new Exception("Bintype already set");
            this.BinType = BinType;
            for (int i = 0; i < BinType.SlotCount; i++)
            {
                Slots.Add(null);
            }
            binSet = true;
        }

        public BinPos GetPosition()
        {
            if (Grid == null) throw new Exception("Grid not set");
            var pos = GetAllPositions();
            if (pos.Count == 0)
                throw new Exception("Bin not found in grid");
            return pos[0];
        }

        public List<BinPos> GetAllPositions()
        {
            if (Grid == null) throw new Exception("Grid not set");
            var outp = new List<BinPos>();
            foreach (var (x, y) in Grid.AllPositions())
            {
                var bin = Grid.Grid[x][y];
                if (bin != null && bin.BinId == BinId)
                {
                    outp.Add(new(x, y));
                }
            }
            return outp;
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

        public bool IsDeletable()
        {
            if (Grid != null) return false;
            if (Slots.Any(s => s != null)) return false;
            return true;
        }
    }
}
