using Backend.Application.DTOs;
using Backend.Application.DTOs.Grid;
using Backend.Application.DTOs.Location;
using Backend.Domain;
using Backend.Exceptions;
using Backend.Infrastructure.Datenbank;
using DBconnector.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Backend.Application.UseCases
{
    public class UcBins
    {
        private readonly RepoDatabase _repo;
        private readonly Dset _data;
        internal UcBins(RepoDatabase repo, Dset data)
        {
            _repo = repo;
            _data = data;
        }


        public List<DTOBinType> GetAllBinTypes()
        {
            return _data.Types
                .Select(t => t.ToDto())
                .ToList();
        }

        public void CreateBinType(int slotCount, int xSize, int ySize)
        {
            foreach (var binType in _data.Types)
            {
                if (binType.SlotCount == slotCount && binType.X == xSize && binType.Y == ySize) return;
            }
            _repo.CreateBinType(_data, slotCount, xSize, ySize);
        }

        public void CreatePart(int InventreeID)
        {
            _repo.CreatePart(_data, InventreeID);
        }


        public List<DTOTreeGrid> GetAllGrids()
        {
            var grids = _data.Root.GetAllGrids();
            return grids.ToTreeDto();
        }
        
        public DTOBin? GetBinById(int binId)
        {
            var bin = _data.findBinbyId(binId);
            if (bin == null) return null;
            return bin.toDto();
        }
        
        public List<DTOBin> GetGridlessBins()
        {
            var data = new List<DTOBin>();
            foreach (var bin in _data.GetAllBins())
            {
                if (bin.Grid != null) continue;
                data.Add(bin.toDto());
            }
            return data;
        }

        public bool CanCreateBin(int BinTypeID, int? GridID)
        {
            if (GridID == null)
            {
                return true;
            }
            var BinPos = FindFreePosition(BinTypeID, (int)GridID);
            return BinPos != null;
        }

        public void CreateBin(int BinTypeID, int? GridID)
        {
            if (GridID == null)
            {
                _repo.CreateBin(BinTypeID);
                return;
            }
            var BinPos = FindFreePosition(BinTypeID, (int)GridID) ?? throw new Exception("No free position for this bin in the grid or Grid not found");

            _repo.CreateBin(BinTypeID, (int)GridID, BinPos.Xpos, BinPos.Ypos);

            _repo.ReloadLocationData(_data);
        }
        
        private BinPos? FindFreePosition(int binTypeId, int gridId)
        {
            var grid = _data.Root.FindGridByID(gridId);
            if (grid == null) return null;
            var BinType = _data.findBinTypebyID(binTypeId);
            var BinPos = grid.FindFreePosForBin(BinType);
            return BinPos;
        }

        public void UpdateBin(int BinId, List<IDtoPart> Parts, int? GridId)
        {
            var bin = _data.findBinbyId(BinId) ?? throw new NotFoundException("Bin", BinId);
            var binGrid = bin.Grid;



            if ((binGrid != null && GridId == null) || (binGrid != null && binGrid.GridId != GridId))
                _repo.RemoveBinfromGrid(BinId, binGrid.GridId);

            if (GridId != null && (binGrid == null || binGrid.GridId != GridId))
            {
                var grid = _data.Root.FindGridByID((int)GridId) ?? throw new NotFoundException("Grid", GridId);
                var BinPos = grid.FindFreePosForBin(bin.BinType)?? throw new Exception("No free position for this bin in the grid");
                _repo.CreateBinPos(BinId, (int)GridId, BinPos.Xpos, BinPos.Ypos);
            }

            // Check Partlist
            bool UpdateParts = false;
            if (bin.Slots.Count != Parts.Count) UpdateParts = true;
            else
            {
                for (int i = 0; i < Parts.Count; i++)
                {
                    var NewPart = Parts[i];
                    if (NewPart.Id == int.MaxValue) NewPart = null;
                    var OldPart = bin.Slots[i];
                    if (NewPart == null || OldPart == null)
                    {
                        if ((NewPart == null) != (OldPart == null))
                            UpdateParts = true;
                        continue;
                    }
                    if (NewPart.Id != OldPart.PartId) UpdateParts = true;
                }
            }

            if (UpdateParts)
            {
                throw new NotImplementedException();
            }

            var newData = _repo.GetData();
            _data.Parts = newData.Parts;
            _data.Types = newData.Types;
            _data.Root = newData.Root;
        }
        
        public void DeleteBin(int binID)
        {
            _repo.DeleteBin(binID);

            var newData = _repo.GetData();
            _data.Parts = newData.Parts;
            _data.Types = newData.Types;
            _data.Root = newData.Root;
        }
    }
}
