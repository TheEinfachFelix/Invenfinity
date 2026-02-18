using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain;
using DBconnector.Models;

namespace Backend.Infrastructure.Mapper
{
    internal static class DBMapper
    {
        public static DLocation mapLocation(Location inlocation, DLocation? parent, Dset data)
        {
            // erstellen des Location Objekts
            var outp = new DLocation(inlocation.LocationId, inlocation.Name ?? "", inlocation.MasterLocationId, parent);
            // Nachliefern von Grids un Childeren da diese eine Referenz auf das Location Objekt benötigen
            var Grids = new List<DGrid>();
            foreach (Grid item in inlocation.Grids)
            {
                Grids.Add(mapGrid(item, outp, data));
            }
            outp.Grids = Grids;

            var Childeren = new List<DLocation>();
            foreach (Location item in inlocation.InverseMasterLocation)
            {
                Childeren.Add(mapLocation(item, outp, data));
            }
            outp.Childeren = Childeren;

            return outp;
        }
        public static DGrid mapGrid(Grid inGrid, DLocation parent, Dset data)
        {
            // erstellen des Grid Objekts
            var outp =  new DGrid(inGrid.GridId, inGrid.Name ?? "", parent, inGrid.LocationId ?? int.MinValue);
            // Nachliefern von Bins da diese eine Referenz auf das Grid Objekt benötigen
            var newGrid = new List<List<DBin>>();
            foreach (var item in inGrid.GridPos)
            {
                if (newGrid[item.X][item.Y] != null) throw new Exception("Grid Pos already filled");
                if (item.Bin == null) throw new Exception("Bin is null");
                newGrid[item.X][item.Y] = mapBin(item.Bin, data, outp);
            }
            outp.Grid = newGrid;

            return outp;
        }
        public static DBin mapBin(Bin inBin, Dset data, DGrid Grid)
        {
            // referenzieren der Parts zu der Slotliste des Bins
            var Slots = new List<DPart>(); 
            foreach (var item in inBin.BinSlots)
            {
                Slots[item.SlotNr] = data.findPartbyID(item.PartId);
            }
            // referenzieren des BinTypes zu dem BinType Objekt des Bins
            var Bintype = data.findBinTypebyID(inBin.BinTypeId);
            // erstellen des Bin Objekts
            return new DBin(inBin.BinId, Slots, Bintype, Grid);
        }

        public static DBinType mapBinType(BinType inType)
        {
            return new DBinType(inType.BinTypeId, inType.SlotCount, inType.X, inType.Y);
        }
        public static DPart mapPart(Part inPart)
        {
            return new DPart(inPart.PartId, inPart.InventreeId);
        }
    }
}
