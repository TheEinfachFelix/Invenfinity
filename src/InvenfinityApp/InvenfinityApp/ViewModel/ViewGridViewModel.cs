using Backend.Application.DTOs;
using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace InvenfinityApp.ViewModel
{
    class ViewGridViewModel
    {

        private readonly UcRoot _root;

        public ObservableCollection<IDotTreeItem> RootItems { get; set; }

        public ViewGridViewModel()
        {
            _root = new UcRoot();

            var rootLocation = _root.Locations.GetLocations();

            RootItems = new ObservableCollection<IDotTreeItem>
            {
                rootLocation
            };
        }

        public void GridClicked(DTOGrid grid)
        {
            // DEMO
            MessageBox.Show($"Grid geklickt: {grid.Name} ({grid.Id})");
        }
    }
}
