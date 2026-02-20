using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace InvenfinityApp.ViewModel
{
    class ViewGridViewModel
    {

        private readonly UcRoot _root;

        public ObservableCollection<object> RootItems { get; set; }

        public ViewGridViewModel()
        {
            _root = new UcRoot();

            var rootLocation = _root.Locations.GetLocations();

            RootItems = new ObservableCollection<object>
            {
                rootLocation
            };
        }
    }
}
