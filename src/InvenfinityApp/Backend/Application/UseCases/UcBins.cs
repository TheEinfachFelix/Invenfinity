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

        public List<IDtoPart> GetAllParts()
        {
            return _data.Parts!.ToDto();
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

        public bool CanCreateBin(int BinTypeID, int? GridID, int? BinId = null)
        {
            if (GridID == null)
            {
                return true;
            }
            var BinPos = FindFreePosition(BinTypeID, (int)GridID, BinId);
            return BinPos != null;
        }

        public void CreateBin(int BinTypeID, int? GridID)
        {
            if (GridID == null)
            {
                _repo.CreateBin(BinTypeID);
                reload();
                return;
            }
            var BinPos = FindFreePosition(BinTypeID, (int)GridID) ?? throw new Exception("No free position for this bin in the grid or Grid not found");

            _repo.CreateBin(BinTypeID, (int)GridID, BinPos.Xpos, BinPos.Ypos);

            reload();
        }
        
        private BinPos? FindFreePosition(int binTypeId, int gridId, int? BinId = null)
        {
            var grid = _data.Root.FindGridByID(gridId);
            if (grid == null) return null;
            var BinType = _data.findBinTypebyID(binTypeId);
            var BinPos = grid.FindFreePosForBin(BinType, BinId);
            return BinPos;
        }

        public void UpdateBin(int BinId, List<IDtoPart> Parts, int? newGridId)
        {
            var bin = _data.findBinbyId(BinId) ?? throw new NotFoundException("Bin", BinId);
            var oldBinGrid = bin.Grid;



            if ((oldBinGrid != null && newGridId == null) || (oldBinGrid != null && oldBinGrid.GridId != newGridId))
                _repo.RemoveBinfromGrid(BinId, oldBinGrid.GridId);

            if (newGridId != null && (oldBinGrid == null || oldBinGrid.GridId != newGridId))
            {
                var grid = _data.Root.FindGridByID((int)newGridId) ?? throw new NotFoundException("Grid", newGridId);
                var BinPos = grid.FindFreePosForBin(bin.BinType, BinId)?? throw new Exception("No free position for this bin in the grid");
                _repo.CreateBinPos(BinId, (int)newGridId, BinPos.Xpos, BinPos.Ypos);
            }

            var changedSlots = GetChangedSlots(bin, Parts);

            if (changedSlots.Count > 0)
            {
                UpdateSlots(BinId, changedSlots);
            }

            reload();
        }

        private List<(int SlotIndex, int? PartId)> GetChangedSlots(DBin bin, List<IDtoPart> newParts)
        {
            var result = new List<(int, int?)>();

            for (int i = 0; i < newParts.Count; i++)
            {
                var newPart = newParts[i];
                int? newPartId = newPart is DTOPartEmpty ? null : newPart.Id;

                var oldSlot = bin.Slots[i];
                int? oldPartId = oldSlot?.PartId;

                if (newPartId != oldPartId)
                    result.Add((i, newPartId));
            }

            return result;
        }

        private void UpdateSlots(int binId, List<(int SlotIndex, int? PartId)> changes)
        {
            foreach (var change in changes)
            {
                _repo.UpdateBinSlot(
                    binId,
                    change.SlotIndex,
                    change.PartId
                );
            }
        }

        public void DeleteBin(int binID)
        {
            _repo.DeleteBin(binID);
            reload();
        }

        private void reload()
        {
            var newData = _repo.GetData();
            _data.Parts = newData.Parts;
            _data.Types = newData.Types;
            _data.Root = newData.Root;
        }
    }
}
