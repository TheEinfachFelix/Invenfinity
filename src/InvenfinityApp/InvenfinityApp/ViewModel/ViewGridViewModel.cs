using Backend.Application.DTOs;
using Backend.Application.DTOs.Location;
using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace InvenfinityApp.ViewModel
{
    public class ViewGridViewModel : INotifyPropertyChanged
    {
        public readonly UcRoot _root;

        private ObservableCollection<IDotTreeItem> _rootItems;
        public ObservableCollection<IDotTreeItem> RootItems
        {
            get => _rootItems;
            set
            {
                _rootItems = value;
                OnPropertyChanged(nameof(RootItems)); // <- sehr wichtig
            }
        }
        private DTOGrid _grid;
        public DTOGrid Grid
        {
            get => _grid;
            private set
            {
                _grid = value;
                OnPropertyChanged(nameof(Grid));
            }
        }

        private int? _selectedGrid = 1;
        public int? SelectedGrid
        {
            get => _selectedGrid;
            set
            {
                if (value != null)
                {
                    _selectedGrid = value;
                    OnPropertyChanged(nameof(SelectedGrid));
                    ReloadGrid();
                }
            }
        }

        private DTOTreeItemEdit? _selectedItemEdit;
        public DTOTreeItemEdit? SelectedItemEdit
        {
            get => _selectedItemEdit;
            set
            {
                _selectedItemEdit = value;
                OnPropertyChanged(nameof(SelectedItemEdit));
            }
        }

        public ViewGridViewModel()
        {
            _root = new UcRoot();
            RootItems = new ObservableCollection<IDotTreeItem>
            {
                _root.Locations.GetLocations()
            };

            ReloadGrid();
        }

        // Edit
        public void UpdateItemEdit(IDotTreeItem item)
        {
            string itemType = item.GetType().Name;
            SelectedItemEdit = _root.Locations.GetEditItem(item.Id, itemType);
        }
        public void SaveItemEdit()
        {
            if (SelectedItemEdit == null) return;
            _root.Locations.EditItem(SelectedItemEdit);
            ReloadGrid();
        }
        public void DeleteItemEdit()
        {
            if (SelectedItemEdit == null) return;
            _root.Locations.DeleteItem(SelectedItemEdit);
            SelectedItemEdit = null;
            ReloadGrid();
        }

        // Grid
        public void MoveBin(DTOBin bin, int newX, int newY)
        {
            _root.Grid.moveBinInGrid(Grid, bin, newX, newY);
            ReloadGrid();
        }
        public bool isAreaFree(int x, int y, DTOBin bin)
        {
            return _root.Grid.isBinMovePossible(Grid, bin, x, y);
        }
        public void ReloadGrid()
        {
            RootItems = new ObservableCollection<IDotTreeItem>
            {
                _root.Locations.GetLocations()
            };
            if (SelectedGrid == null) return;
            Grid = _root.Grid.getGridByID((int)SelectedGrid);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
