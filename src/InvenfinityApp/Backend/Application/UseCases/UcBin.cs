using Backend.Application.DTOs.Grid.Edit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases
{
    public class UcBin
    {
        private readonly UcRoot _root;
        internal UcBin(UcRoot root)
        {
            _root = root;
        }

        public void CreateBinType(int slotCount, int xSize, int ySize)
        {
            _root.RepoDatabase.CreateBinType(_root.Data, slotCount, xSize, ySize);
        }
        public void CreatePart(int InventreeID)
        {
            _root.RepoDatabase.CreatePart(_root.Data, InventreeID);
        }

        public DTOEditBin GetBinById(int binId)
        {
            var bin = _root.Data.findBinbyId(binId);
            return GridEditFactory.CreateBin(bin);
        }
    }
}
