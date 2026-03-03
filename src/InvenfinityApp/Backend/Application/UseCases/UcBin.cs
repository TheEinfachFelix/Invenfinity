using Backend.Application.DTOs;
using Backend.Application.DTOs.Grid.Edit;
using Backend.Application.DTOs.Location;
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

        public List<DTOEditBinType> GetAllBinTypes()
        {
            var binTypes = _root.Data.Types;
            List<DTOEditBinType> dtoBinTypes = new List<DTOEditBinType>();
            foreach (var binType in binTypes)
            {
                dtoBinTypes.Add(GridEditFactory.CreateBinType(binType));
            }
            return dtoBinTypes;
        }
        public List<DTOTreeGrid> GetAllGrids()
        {
            var grids = _root.Data.Root.GetAllGrids();
            return LocationFactory.CreateGridList(grids);
        }

        public void CreateBin(int BinTypeID, int? GridID)
        {
            throw new NotImplementedException();
                      // _root.RepoDatabase.CreateBin(_root.Data, BinTypeID, GridID);

        }
    }
}
