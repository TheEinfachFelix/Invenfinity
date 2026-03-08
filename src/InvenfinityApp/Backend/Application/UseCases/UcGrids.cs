using Backend.Application.DTOs.Grid;
using Backend.Domain;
using Backend.Exceptions;
using Backend.Infrastructure.Datenbank;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases
{
    public class UcGrids
    {
        private readonly RepoDatabase _repo;
        private readonly Dset _data;
        internal UcGrids(RepoDatabase repo, Dset data)
        {
            _repo = repo;
            _data = data;
        }
        public DTOGrid GetGridByID(int id)
        {
            var Dgrid = _data.Root.FindGridByID(id);
            if (Dgrid == null) throw new NotFoundException("Grid",id);

            return Dgrid.ToDto();
        }
        public void MoveBinInGrid(int gridId, DTOBin inBin, int X, int Y)
        {
            var Dgrid = _data.Root.FindGridByID(gridId) ?? throw new NotFoundException("Grid", gridId);
            var Dbin = Dgrid.FindBinByID(inBin.Id) ?? throw new NotFoundException("Bin", inBin.Id);
            Dgrid.MoveBin(Dbin, X, Y);
            _repo.UpdateGrid(Dgrid);
        }
        public bool IsBinMovePossible(DTOGrid grid, DTOBin inBin, int X, int Y)
        {
            var Dgrid = _data.Root.FindGridByID(grid.GridId) ?? throw new NotFoundException("Grid", grid.GridId);
            var Dbin = Dgrid.FindBinByID(inBin.Id) ?? throw new NotFoundException("Bin", inBin.Id);
            return Dgrid.IsAreaFree(X, Y, Dbin.BinType, inBin.Id);
        }
    }
}
