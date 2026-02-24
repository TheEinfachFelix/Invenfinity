using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs
{
    public class DTOBin
    {
        internal DTOBin(int BinId, int Xpos, int Ypos, int Width, int Height, List<IDotPart> Parts)
        {
            this.BinId = BinId;
            this.X = Xpos;
            this.Y = Ypos;
            this.WidthCells = Width;
            this.HeightCells = Height;
            this.Parts = Parts;
        }
        public int BinId { get; }
        public int X { get;}                      // Top-left Spalte (0-basiert)
        public int Y { get; }                      // Top-left Reihe (0-basiert)
        public int WidthCells { get; }             // Breite in Zellen
        public int HeightCells { get; }            // Höhe in Zellen
        public List<IDotPart> Parts { get; }

        // Visual hints (keine Domain-Logik, nur für UI)
        public int ZIndex { get; } = 0;            // Rendering-Ebene
        public string? BackgroundBrushKey { get; } // z.B. "BinBackgroundBrush"
        public string? BorderBrushKey { get; }     // z.B. "BinBorderBrush"
    }
}
