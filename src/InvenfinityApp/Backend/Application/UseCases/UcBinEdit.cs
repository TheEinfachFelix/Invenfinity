using Backend.Application.DTOs;
using Backend.Application.DTOs.Grid.Edit;
using Backend.Application.DTOs.Grid;
using DBconnector.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases
{
    public class UcBinEdit
    {
        private readonly UcRoot _root;
        internal UcBinEdit(UcRoot root)
        {
            _root = root;
        }

        public List<DTOEditBin> getGridlessBins()
        {
            var data = new List<DTOEditBin>();
            foreach (var bin in _root.Data.GetAllBins())
            {
                if (bin.Grid != null) continue;
                data.Add(GridEditFactory.CreateBin(bin));
            }
            return data;
        }
        public void updateBin(int BinId, List<DTOPart> Parts, int? GridId)
        {
            var bin = _root.Data.findBinbyId(BinId);
            if (bin == null) throw new Exception("Bin Not found");
            var binGrid = bin.Grid;

            

            if ((binGrid != null && GridId == null) || (binGrid != null && binGrid.GridId != GridId))
                _root.RepoDatabase.RemoveBinfromGrid(BinId, binGrid.GridId);

            if ((binGrid == null && GridId != null) || (binGrid == null && GridId != null) || (binGrid.GridId != GridId && GridId != null))
            {
                var grid = _root.Data.Root.FindGridByID((int)GridId);
                var BinPos = grid.FindFreePosForBin(bin.BinType);
                if (BinPos == null)
                {
                    throw new Exception("No free position for this bin in the grid");
                }
                _root.RepoDatabase.CreateBinPos(BinId, (int)GridId, BinPos.Xpos, BinPos.Ypos);
            }

            // Check Partlist
            bool UpdateParts = false;
            if (bin.Slots.Count != Parts.Count) UpdateParts = true;
            else
            {
                for (int i = 0; i < Parts.Count; i++) 
                {
                    var NewPart = Parts[i];
                    if (NewPart.Id == 0) NewPart = null;
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


            _root.Data = _root.RepoDatabase.GetData();
        }
        public void deleteBin(int binID)
        {
            _root.RepoDatabase.DeleteBin(binID);
            _root.Data = _root.RepoDatabase.GetData();
        }
    }
}
