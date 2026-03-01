using Backend.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases
{
    public class UcGrid
    {
        private readonly UcRoot _root;
        internal UcGrid(UcRoot root)
        {
            _root = root;
        }
        public DTOGrid getGridByID(int id)
        {
            var Dgrid = _root.Data.Root.FindGridByID(id);
            if (Dgrid == null) throw new Exception("Grid not found");

            return GridFactory.CreateGrid(Dgrid);
        }
        public void moveBinInGrid(DTOGrid grid, DTOBin inBin, int X, int Y)
        {
            var Dgrid = _root.Data.Root.FindGridByID(grid.GridId) ?? throw new Exception("Grid not found");
            var Dbin = Dgrid.FindBinByID(inBin.BinId);
            if (Dbin == null) throw new Exception("Bin not found in grid");
            Dgrid.MoveBin(Dbin, X, Y);
            _root.RepoDatabase.UpdateGrid(Dgrid);
        }
        public bool isBinMovePossible(DTOGrid grid, DTOBin inBin, int X, int Y)
        {
            var Dgrid = _root.Data.Root.FindGridByID(grid.GridId) ?? throw new Exception("Grid not found");
            var Dbin = Dgrid.FindBinByID(inBin.BinId);
            if (Dbin == null) throw new Exception("Bin not found in grid");
            return Dgrid.IsAreaFree(X, Y, Dbin.BinType, inBin.BinId);
        }
    }
}
