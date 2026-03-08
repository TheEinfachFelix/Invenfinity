using Backend.Application.DTOs.Grid;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs
{
    public class DTOBin : IDtoDropdownElement 
    {
        internal DTOBin(int Id, int? GridId, int Xpos, int Ypos, List<IDotPart> Parts, DTOBinType BinType, bool isDeletable)
        {
            this.Id = Id;
            this.GridId = GridId;
            this.X = Xpos;
            this.Y = Ypos;
            this.Parts = Parts;
            this.BinType = BinType;
            this.isDeletable = isDeletable;
        }
        public string Name => Id.ToString();
        public int Id { get; }
        public int? GridId { get; }
        public DTOBinType BinType { get; }
        public bool isDeletable { get; }

        // Grid Drawing
        public int X { get;}
        public int Y { get; }
        public int WidthCells => BinType.xSize;
        public int HeightCells => BinType.ySize;
        public List<IDotPart> Parts { get; }

        // Visual hints (keine Domain-Logik, nur für UI)
        public int ZIndex { get; } = 0;            // Rendering-Ebene
        public string? BackgroundBrushKey { get; } // z.B. "BinBackgroundBrush"
        public string? BorderBrushKey { get; }     // z.B. "BinBorderBrush"
    }
}
