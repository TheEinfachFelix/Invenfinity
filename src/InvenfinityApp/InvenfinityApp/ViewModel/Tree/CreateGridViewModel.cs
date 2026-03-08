using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvenfinityApp.ViewModel.Tree
{
    public class CreateGridViewModel
    {
        private readonly UcRoot _root;

        public CreateGridViewModel(UcRoot root)
        {
            _root = root;
        }

        public event Action? GridCreated;

        public void CreateGrid(string name, int parentID, int xSize, int ySize)
        {
            _root.Locations.CreateGrid(name, parentID, xSize, ySize);
            GridCreated?.Invoke();
        }
    }
}
