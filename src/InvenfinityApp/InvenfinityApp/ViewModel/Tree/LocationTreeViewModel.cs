using Backend.Application.DTOs;
using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace InvenfinityApp.ViewModel.Tree
{
    public class LocationTreeViewModel : INotifyPropertyChanged
    {
        private UcRoot _root;

        private ObservableCollection<IDotTreeItem> _rootItems;
        public ObservableCollection<IDotTreeItem> RootItems
        {
            get => _rootItems;
            set
            {
                _rootItems = value;
                OnPropertyChanged(nameof(RootItems));
            }
        }
        public LocationTreeViewModel(UcRoot root)
        {
            _root = root;
            _rootItems = new ObservableCollection<IDotTreeItem>
            {
                _root.Locations.GetLocations()
            };
        }

        public void ReloadTree()
        {
            RootItems = new ObservableCollection<IDotTreeItem>
            {
                _root.Locations.GetLocations()
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
