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
        private IDtoTreeEditItem? _selectedItemEdit;
        public IDtoTreeEditItem? SelectedItemEdit
        {
            get => _selectedItemEdit;
            set
            {
                _selectedItemEdit = value;
                OnPropertyChanged(nameof(SelectedItemEdit));
            }
        }


        public event Action? TreeChanged;
        public LocationEditViewModel(UcRoot root)
        {
            _root = root;
        }

        public void UpdateItemEdit(IDtoTreeItem item)
        {
            SelectedItemEdit = _root.Locations.GetEditItem(item);
        }
        public void SaveItemEdit()
        {
            if (SelectedItemEdit == null) return;
            _root.Locations.EditItem(SelectedItemEdit);
            TreeChanged?.Invoke();
        }
        public void DeleteItemEdit()
        {
            if (SelectedItemEdit == null) return;
            _root.Locations.DeleteItem(SelectedItemEdit);
            SelectedItemEdit = null;
            TreeChanged?.Invoke();
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
