using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvenfinityApp.ViewModel.Part
{
    class CreateBinViewModel
    {
        private UcRoot root;
        public CreateBinViewModel(UcRoot root)
        {
            this.root = root;
        }
        public event Action? PartsChanged;
        public void CreateBin()
        {
            //root.Bin.CreatePart(InventreeID);
            //PartsChanged?.Invoke();
        }
    }
}
