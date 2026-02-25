using DBconnector.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Backend.Application.DTOs
{
    public class DTOGrid
    {
        internal DTOGrid(int GridId, int WidthCells, int HeightCells, IReadOnlyList< DTOBin > Bins)
        {
            this.GridId = GridId;
            this.WidthCells = WidthCells;
            this.HeightCells = HeightCells;
           // this.CellSizeMm = CellSizeMm;
            this.Bins = Bins;
        }
        public int GridId { get; }
        public int WidthCells { get; }
        public int HeightCells { get; }
        //public double CellSizeMm { get; }
        public IReadOnlyList< DTOBin > Bins { get; }
    }
}
