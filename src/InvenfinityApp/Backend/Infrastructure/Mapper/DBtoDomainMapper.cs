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
    internal static class DBtoDomainMapper
    {
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
                newGrid[item.X][item.Y] = mapBin(item.Bin, data);
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
        public static DBin mapBin(Bin inBin, Dset data)
        { 
            // referenzieren des BinTypes zu dem BinType Objekt des Bins
            var Bintype = data.findBinTypebyID(inBin.BinTypeId);

            // erstellen des Bin Objekts
            var outp = new DBin(inBin.BinId, Bintype);

            // hinzufügen der Parts zu den Slots des Bin Objekts
            foreach (var item in inBin.BinSlots)
            {
                outp.AddPart(data.findPartbyID(item.PartId), item.SlotNr);
            }

            return outp;
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
