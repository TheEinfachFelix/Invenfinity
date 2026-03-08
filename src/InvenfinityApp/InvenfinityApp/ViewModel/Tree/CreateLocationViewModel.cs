using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvenfinityApp.ViewModel.Tree
{
    public class CreateLocationViewModel
    {
        private readonly UcRoot _root;
        public CreateLocationViewModel(UcRoot root)
        {
            _root = root;
        }

        public event Action? LocationCreated;

        public void CreateLocation(string name, int parentID)
        {
            _root.Locations.CreateLocation(name, parentID);
            LocationCreated?.Invoke();
        }
    }
}
