using Backend.Application.DTOs;
using Backend.Application.DTOs.Grid;
using Backend.Application.DTOs.Location;
using Backend.Domain;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Backend.Application.UseCases
{
    public class UcBinCreate
    {
        private readonly UcRoot _root;
        internal UcBinCreate(UcRoot root)
        {
            _root = root;
        }

        public void CreateBinType(int slotCount, int xSize, int ySize)
        {
            foreach (var binType in _root.Data.Types)
            {
                if (binType.SlotCount == slotCount && binType.X == xSize && binType.Y == ySize) return;
            }
            _root.RepoDatabase.CreateBinType(_root.Data, slotCount, xSize, ySize);
        }
        public void CreatePart(int InventreeID)
        {
            _root.RepoDatabase.CreatePart(_root.Data, InventreeID);
        }

        public DTOBin? GetBinById(int binId)
        {
            var bin = _root.Data.findBinbyId(binId);
            if (bin == null) return null;
            return bin.toDto();
        }

        public List<DTOBinType> GetAllBinTypes()
        {
            var binTypes = _root.Data.Types;
            List<DTOBinType> dtoBinTypes = new List<DTOBinType>();
            foreach (var binType in binTypes)
            {
                dtoBinTypes.Add(binType.ToDto());
            }
            return dtoBinTypes;
        }
        public List<DTOTreeGrid> GetAllGrids()
        {
            var grids = _root.Data.Root.GetAllGrids();
            return grids.ToTreeDto();
        }

        public void CreateBin(int BinTypeID, int? GridID)
        {
            if (GridID == null)
            {
                _root.RepoDatabase.CreateBin(BinTypeID);
                return;
            }

            var grid = _root.Data.Root.FindGridByID((int)GridID);
            if (grid == null)
            {
                throw new Exception("Grid not found");
            }
            var BinType = _root.Data.findBinTypebyID(BinTypeID);
            var BinPos = grid.FindFreePosForBin(BinType);
            if (BinPos == null)
            {
                throw new Exception("No free position for this bin in the grid");
            }
            _root.RepoDatabase.CreateBin(BinTypeID, (int)GridID, BinPos.Xpos, BinPos.Ypos);

            _root.RepoDatabase.ReloadLocationData(_root.Data);
        }
        public bool CanCreateBin(int BinTypeID, int? GridID)
        {
            if (GridID == null)
            {
                return true;
            }
            var grid = _root.Data.Root.FindGridByID((int)GridID);
            if (grid == null) return false;
            var BinType = _root.Data.findBinTypebyID(BinTypeID);
            var BinPos = grid.FindFreePosForBin(BinType);
            return BinPos != null;
        }
    }
}
