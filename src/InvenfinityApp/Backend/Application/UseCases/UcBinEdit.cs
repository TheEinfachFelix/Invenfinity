using Backend.Application.DTOs;
using Backend.Application.DTOs.Grid.Edit;
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

        public List<DTOEditBin> getBins()
        {
            var data = new List<DTOEditBin>();
            foreach (var bin in _root.Data.GetAllBins())
            {
                data.Add(GridEditFactory.CreateBin(bin));
            }
            return data;
        }
    }
}
