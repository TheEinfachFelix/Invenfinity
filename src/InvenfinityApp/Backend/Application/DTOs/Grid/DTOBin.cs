using Backend.Application.DTOs.Grid;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid
{
    public class DTOBin : IDtoDropdownElement 
    {
        internal DTOBin(int Id, int? GridId, int Xpos, int Ypos, List<IDtoPart> Parts, DTOBinType BinType, bool isDeletable)
        {
            this.Id = Id;
            this.GridId = GridId;
            this.X = Xpos;
            this.Y = Ypos;
            this.Parts = Parts;
            this.BinType = BinType;
            this.isDeletable = isDeletable;
        }
        public string Name => $"Bin {Id}";
        public int Id { get; }
        public int? GridId { get; }
        public DTOBinType BinType { get; }
        public bool isDeletable { get; }

        // Grid Drawing
        public int X { get;}
        public int Y { get; }
        public int WidthCells => BinType.XSize;
        public int HeightCells => BinType.YSize;
        public IReadOnlyList<IDtoPart> Parts { get; }

    }
}
