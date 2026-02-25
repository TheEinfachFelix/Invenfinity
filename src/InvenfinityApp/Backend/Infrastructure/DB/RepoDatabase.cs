using Backend.Domain;
using Backend.Infrastructure.Mapper;
using DBconnector;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Datenbank
{
    public class RepoDatabase
    {
        AppDbContext context;
        public RepoDatabase(AppDbContext context) { this.context = context; }

        internal Dset GetData()
        {
            var outp = new Dset();
            //  Get the bin Types
            foreach (var item in context.BinTypes.ToList())
            {
                outp.Types.Add(DBtoDomainMapper.mapBinType(item));
            }
            // get the parts
            foreach (var item in context.Parts.ToList())
            {
                outp.Parts.Add(DBtoDomainMapper.mapPart(item));
            }

            // get the rest of the Data
            var inloc = context.Locations.Single(l => l.LocationId == 1);
            LoadGridsRecursively(inloc);


            if (inloc == null) throw new Exception("The root Location was not found");
            outp.Root = DBtoDomainMapper.mapLocation(inloc, null, outp);
            return outp;
        }

        private void LoadGridsRecursively(Location loc)
        {
            // Grids und GridPos + Bin + BinType + BinSlots + Part laden
            context.Entry(loc)
                .Collection(l => l.Grids)
                .Query()
                .Include(g => g.GridPos)
                    .ThenInclude(gp => gp.Bin)
                        .ThenInclude(b => b.BinType)
                .Include(g => g.GridPos)
                    .ThenInclude(gp => gp.Bin)
                        .ThenInclude(b => b.BinSlots)
                            .ThenInclude(bs => bs.Part)
                .Load();

            // alle Kinder laden
            context.Entry(loc)
                .Collection(l => l.InverseMasterLocation)
                .Load();

            foreach (var child in loc.InverseMasterLocation)
            {
                LoadGridsRecursively(child);
            }
        }

        internal void UpdateLocation(DLocation inDlocation)
        {
            var dbLocation = context.Locations.Find(inDlocation.LocationId) ?? throw new Exception($"Location with ID {inDlocation.LocationId} not found");
            throw new NotImplementedException();
            dbLocation.Name = inDlocation.Name;
            if (inDlocation.Parent != null)
            {
                dbLocation.MasterLocationId = inDlocation.Parent.LocationId;
            }
            else
            {
                dbLocation.MasterLocationId = null;
            }
            context.SaveChanges();
        }
        internal void UpdateGrid(DGrid inDgrid)
        {
            var dbGrid = context.Grids.Find(inDgrid.GridId) ?? throw new Exception($"Grid with ID {inDgrid.GridId} not found");
            dbGrid.LocationId = inDgrid.LocationId;
            dbGrid.Name = inDgrid.Name;
            dbGrid.Xmax = inDgrid.Xmax;
            dbGrid.Ymax = inDgrid.Ymax;
            // Grid Pos
            var dbGridPosList = dbGrid.GridPos.ToList();
            var inGridBin = inDgrid.GetAllBinsInGrid();
            // checken ob die bins im Grid in der nm tabelle sind
            foreach (var inGridPos in inGridBin)
            {
                UpdateBin(inGridPos);
                if (dbGridPosList.Any(gp => gp.BinId == inGridPos.BinId))
                {
                    // BinPos existiert bereits, nichts tun
                    continue;
                }
                else
                {
                    // BinPos hinzufügen
                    dbGrid.GridPos.Add(new GridPo { GridId = dbGrid.GridId, BinId = inGridPos.BinId });
                }
            }
            // irrelevante Bins entfernen
            foreach (var dbGridPos in dbGridPosList)
            {
                if (!inGridBin.Any(gp => gp.BinId == dbGridPos.BinId))
                {
                    // BinPos entfernen
                    context.GridPos.Remove(dbGridPos);
                }
            }

            // Positionen updaten
            foreach (var inGridPos in inGridBin)
            {
                var dbGridPos = dbGridPosList.FindAll(gp => gp.BinId == inGridPos.BinId);
                var inBinPos = inDgrid.GetAllBinPosInGrid(inGridPos);
                if (dbGridPos.Count > inBinPos.Count)
                {
                    dbGridPos.RemoveAt(dbGridPos.Count - 1);
                }
                if (dbGridPos.Count < inBinPos.Count)
                {
                    dbGridPos.Add(new GridPo { GridId = dbGrid.GridId, BinId = inGridPos.BinId });
                }
                for (int i = 0; i < dbGridPos.Count; i++)
                {
                    dbGridPos[i].X = inBinPos[i].Xpos;
                    dbGridPos[i].Y = inBinPos[i].Ypos;
                }
            }


            context.SaveChanges();
        }
        
        internal void UpdateBin(DBin inDBin)
        {
            var dbBin = context.Bins.Find(inDBin.BinId) ?? throw new Exception($"Bin with ID {inDBin.BinId} not found");
            dbBin.BinTypeId = inDBin.BinType.BinTypeId;
            UpdateBinType(inDBin.BinType);
            for (int i = 0; i < inDBin.BinType.SlotCount; i++)
            {
                var dbBinSlot = dbBin.BinSlots.SingleOrDefault(bs => bs.SlotNr == i);
                var inBinSlot = inDBin.Slots[i];
                if (dbBinSlot == null && inBinSlot != null)
                {
                    // Neuer BinSlot
                    dbBin.BinSlots.Add(new BinSlot { SlotNr = i, PartId = inBinSlot.PartId, BinId = dbBin.BinId });
                }
                else if (dbBinSlot != null && inBinSlot == null)
                {
                    // BinSlot entfernen
                    context.BinSlots.Remove(dbBinSlot);
                }
                else if (dbBinSlot != null && inBinSlot != null)
                {
                    // BinSlot aktualisieren
                    dbBinSlot.PartId = inBinSlot.PartId;
                    UpdatePart(inBinSlot);
                }
            }

            context.SaveChanges();
        }
        internal void UpdateBinType(DBinType inDBinType)
        {
            var dbBinType = context.BinTypes.Find(inDBinType.BinTypeId) ?? throw new Exception($"BinType with ID {inDBinType.BinTypeId} not found");
            dbBinType.SlotCount = inDBinType.SlotCount;
            dbBinType.X = inDBinType.X;
            dbBinType.Y = inDBinType.Y;
            // BinType
            context.SaveChanges();
        }
        internal void UpdatePart(DPart inDPart)
        {
            var dbPart = context.Parts.Find(inDPart.PartId) ?? throw new Exception($"Part with ID {inDPart.PartId} not found");
            dbPart.InventreeId = inDPart.InventreeId;
            // Part
            context.SaveChanges();
        }

    }
}
