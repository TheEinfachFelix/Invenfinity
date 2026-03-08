using Backend.Application.DTOs.Grid;
using Backend.Domain;
using DBconnector.Models;
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

        public static DTOBin CreateBin(DBin bin)
        {
            var type = CreateBinType(bin.BinType);
            var parts = CreatePartList(bin.Slots);
            int? gridId = bin.Grid != null ? bin.Grid.GridId : null;
            var binPos = bin.Grid != null ? bin.GetPosition() : new BinPos(0,0);
            return new(bin.BinId, gridId, binPos.Xpos, binPos.Ypos, parts, type, bin.IsDeletable());
        }
        public static DTOBinType CreateBinType(DBinType binType)
        {
            return new(binType.BinTypeId, binType.SlotCount, binType.X, binType.Y);
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
