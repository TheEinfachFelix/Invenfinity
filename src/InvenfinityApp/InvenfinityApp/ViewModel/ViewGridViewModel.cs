using Backend.Application.DTOs;
using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace InvenfinityApp.ViewModel
{
    class ViewGridViewModel : INotifyPropertyChanged
    {
        public readonly UcRoot _root;

        public ObservableCollection<IDotTreeItem> RootItems { get; set; }

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

        public ViewGridViewModel()
        {
            _root = new UcRoot();
            RootItems = new ObservableCollection<IDotTreeItem>
        {
            _root.Locations.GetLocations()
        };

            ReloadGrid();
        }

        public void MoveBin(DTOBin bin, int newX, int newY)
        {
            _root.Grid.moveBinInGrid(Grid, bin, newX, newY);
            ReloadGrid();
        }

        public void ReloadGrid()
        {
            Grid = _root.Grid.getGridByID(1);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
