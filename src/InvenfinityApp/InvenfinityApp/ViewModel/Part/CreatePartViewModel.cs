using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvenfinityApp.ViewModel.Part
{
    public class CreatePartViewModel
    {
        private UcRoot root;
        public CreatePartViewModel(UcRoot root)
        {
            this.root = root;
        }
        public event Action? PartsChanged;
        public void CreatePart(int InventreeID)
        {
            root.Bins.CreatePart(InventreeID);
            PartsChanged?.Invoke();
        }
    }
}
