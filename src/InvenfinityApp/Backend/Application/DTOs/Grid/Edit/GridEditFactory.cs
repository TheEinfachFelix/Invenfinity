using Backend.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid.Edit
{
    internal static class GridEditFactory
    {
        public static DTOEditBin CreateBin(DBin bin)
        {
            var type = CreateBinType(bin.BinType);
            var parts = new List<DTOEditPart>();
            foreach (var item in bin.Slots)
            {
                var newPart = CreateEmtyPart();
                if (item != null)
                    newPart = CreatePart(item);
                parts.Add(newPart);
            }
            int? gridId = bin.Grid != null ? bin.Grid.GridId : null;
            return new(bin.BinId, gridId, parts, type);
        }
        public static DTOEditBin CreateEmtyBin()
        {
            return new(-1, -1, new(), CreateEmtyBinType());
        }
        public static DTOEditBinType CreateBinType(DBinType binType)
        {
            return new (binType.BinTypeId, binType.SlotCount, binType.X, binType.Y);
        }
        public static DTOEditBinType CreateEmtyBinType()
        {
            return new (-1, -1, -1, -1);
        }
        public static DTOEditPart CreateEmtyPart()
        {
            return new (0);
        }
        public static DTOEditPart CreatePart(DPart part)
        {
            return new (part.PartId);
        }
    }
}
