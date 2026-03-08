using Backend.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs
{
    internal static class GridFactory
    {
        public static DTOPart CreatePart(DPart inPart)
        {
            return new DTOPart(inPart.PartId);
        }

        public static List<IDotPart> CreatePartList(List<DPart?> inParts)
        {
            var outp = new List<IDotPart>();
            foreach (var item in inParts)
            {
                if (item is null) 
                { 
                    outp.Add(DTOPartEmpty.Instance); 
                    continue;
                }
                outp.Add(CreatePart(item));
            }
            return outp;
        }

        public static DTOBin CreateBin(DBin inBin)
        {
            var Pos = inBin.GetPosition();
            if (Pos == null) throw new Exception("Bin not in Grid");
            var type = inBin.BinType;
            var parts = CreatePartList(inBin.Slots);
            return new DTOBin(inBin.BinId, Pos.Xpos, Pos.Ypos, type.X, type.Y, parts);
        }

        public static DTOGrid CreateGrid(DGrid inGrid)
        {
            var outp = new List<DTOBin>();
            foreach (var item in inGrid.Grid)
            {
                foreach (var bin in item)
                {
                    if (bin is not null)
                    {
                        outp.Add(CreateBin(bin));
                    }
                }
            }
            return new DTOGrid(inGrid.GridId ,inGrid.Xmax, inGrid.Ymax, outp);
        }
    }
}
