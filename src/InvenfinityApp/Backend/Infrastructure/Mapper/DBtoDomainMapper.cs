using Backend.Domain;
using DBconnector.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backend.Infrastructure.Mapper
{
    internal static class DBtoDomainMapper
    {
        private static Dictionary<int, DBin> binCache = new();
        private static Dictionary<int, DBinType> binTypeCache = new();
        internal static void ResetCaches()
        {
            binCache.Clear();
            binTypeCache.Clear();
        }
        public static DLocation mapLocation(Location inlocation, DLocation? parent, Dset data)
        {
            // erstellen des Location Objekts
            var outp = new DLocation(inlocation.LocationId, inlocation.Name ?? "", parent);
            // Nachliefern von Grids un Childeren da diese eine Referenz auf das Location Objekt benötigen
            foreach (Grid item in inlocation.Grids)
            {
                // durch den Construcker von Dgrid werden die schon automatisch der Liste Hinzugefügt
                mapGrid(item, outp, data);
            }

            foreach (Location item in inlocation.InverseMasterLocation)
            {
                // Durch den Constructor wird es automtisch der liste hinzugefügt
                mapLocation(item, outp, data);
            }

            return outp;
        }
        public static DGrid mapGrid(Grid inGrid, DLocation parent, Dset data)
        {
            // erstellen des Grid Objekts
            var outp =  new DGrid(inGrid.GridId, inGrid.Name ?? "", parent, inGrid.Xmax, inGrid.Ymax);
            // Nachliefern von Bins da diese eine Referenz auf das Grid Objekt benötigen
            // Setup der Liste
            var newGrid = outp.CreateGridMatrix();
            // Hinzufügen der Bins zu einer temp Liste
            foreach (var item in inGrid.GridPos)
            {
                if (newGrid[item.X][item.Y] != null) throw new Exception("Grid Pos already filled");
                if (item.Bin == null) throw new Exception("Bin is null");
                newGrid[item.X][item.Y] = binCache[item.Bin.BinId];
            }
            // Iterieren über die temp liste und hinzufügen der Bins zum Grid Objekt
            var addedIds = new HashSet<int>();
            for (int X = 0; X < outp.Xmax; X++)
            {
                for (int Y = 0; Y < outp.Ymax; Y++)
                {
                    var bin = newGrid[X][Y];
                    if (bin == null) continue;
                    if (addedIds.Contains(bin.BinId)) continue;
                    outp.AddBin(bin, X, Y);
                    addedIds.Add(bin.BinId);
                }
            }

            return outp;
        }
        public static DBin mapBin(Bin inBin)
        {
            if (binCache.TryGetValue(inBin.BinId, out var existing))
            {
                return existing;
            }

            // erstellen des Bin Objekts
            var outp = new DBin(inBin.BinId);
            outp.SetBinType(binTypeCache[inBin.BinTypeId]);

            binCache[inBin.BinId] = outp;

            return outp;
        }

        public static DBinType mapBinType(BinType inType)
        {
            var outp = new DBinType(inType.BinTypeId, inType.SlotCount, inType.X, inType.Y);
            binTypeCache[outp.BinTypeId] = outp;
            foreach (var bin in inType.Bins)
            {
                outp.Bins.Add(mapBin(bin));
            }
            return outp;
        }
        public static DPart mapPart(Part inPart)
        {
            var outp = new DPart(inPart.PartId, inPart.InventreeId);
            // Hinzufügen zu den Bins
            foreach (var item in inPart.BinSlots)
            {
                binCache[item.BinId].AddPart(outp, item.SlotNr);
            }
            return outp;
        }
    }
}