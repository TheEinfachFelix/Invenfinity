using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvenfinityApp.ViewModel.Grid
{
    public class CreateBinTypeViewModel
    {
        private UcRoot root;
        public CreateBinTypeViewModel(UcRoot root)
        {
            this.root = root;
        }
        public event Action? BinTypeChanged;

        public void CreateBinType(int slotCount, int xSize, int ySize)
        {
            root.Bin.CreateBinType(slotCount, xSize, ySize);
            BinTypeChanged?.Invoke();
        }
    }
}
