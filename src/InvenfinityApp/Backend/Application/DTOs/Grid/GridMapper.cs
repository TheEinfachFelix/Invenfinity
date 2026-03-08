using Backend.Application.DTOs.Grid;
using Backend.Domain;
using DBconnector.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid
{
    internal static class GridMapper
    {
        public static DTOBin toDto(this DBin bin)
        {
            var type = bin.BinType.ToDto();
            var parts = bin.Slots.ToDto();
            int? gridId = bin.Grid != null ? bin.Grid.GridId : null;
            var binPos = bin.Grid != null ? bin.GetPosition() : new BinPos(0,0);
            return new(bin.BinId, gridId, binPos.Xpos, binPos.Ypos, parts, type, bin.IsDeletable());
        }

        public static DTOBinType ToDto(this DBinType binType)
        {
            return new(binType.BinTypeId, binType.SlotCount, binType.X, binType.Y);
        }
        public static DTOPart toDto(this DPart inPart)
        {
            return new DTOPart(inPart.PartId);
        }

        public static List<IDtoPart> ToDto(this List<DPart?> inParts)
        {
            var outp = new List<IDtoPart>();
            foreach (var item in inParts)
            {
                if (item is null)
                {
                    outp.Add(DTOPartEmpty.Instance);
                    continue;
                }
                outp.Add(item.toDto());
            }
            return outp;
        }

        public static DTOGrid ToDto(this DGrid inGrid)
        {
            var outp = new List<DTOBin>();
            foreach (var item in inGrid.Grid)
            {
                foreach (var bin in item)
                {
                    if (bin is not null)
                    {
                        outp.Add(bin.toDto());
                    }
                }
            }
            return new DTOGrid(inGrid.GridId ,inGrid.Xmax, inGrid.Ymax, outp);
        }
    }
}
