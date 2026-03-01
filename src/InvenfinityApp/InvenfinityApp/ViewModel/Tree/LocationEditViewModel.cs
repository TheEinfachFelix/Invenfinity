using Backend.Application.DTOs;
using Backend.Application.DTOs.Location;
using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace InvenfinityApp.ViewModel.Tree
{
    public class LocationEditViewModel : INotifyPropertyChanged
    {
        private readonly UcRoot _root;
        private IDotTreeEditItem? _selectedItemEdit;
        public IDotTreeEditItem? SelectedItemEdit
        {
            get => _selectedItemEdit;
            set
            {
                _selectedItemEdit = value;
                OnPropertyChanged(nameof(SelectedItemEdit));
            }
        }
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

        public event Action? TreeCreated;
        public LocationEditViewModel(UcRoot root)
        {
            _root = root;
            _rootItems = new ObservableCollection<IDotTreeItem>
            {
                _root.Locations.GetLocations()
            };
        }

        public void UpdateItemEdit(IDotTreeItem item)
        {
            SelectedItemEdit = _root.Locations.Edit.GetEditItem(item);
        }
        public void SaveItemEdit()
        {
            if (SelectedItemEdit == null) return;
            _root.Locations.Edit.EditItem(SelectedItemEdit);
            TreeCreated?.Invoke();
        }
        public void DeleteItemEdit()
        {
            if (SelectedItemEdit == null) return;
            _root.Locations.Edit.DeleteItem(SelectedItemEdit);
            SelectedItemEdit = null;
            TreeCreated?.Invoke();
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
