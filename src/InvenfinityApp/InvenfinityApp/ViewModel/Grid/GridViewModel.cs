using Backend.Application.DTOs.Grid;
using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace InvenfinityApp.ViewModel.Grid
{
    public class GridViewModel : INotifyPropertyChanged
    {
        private readonly UcRoot _root;

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
                    GridUpdated?.Invoke();
                }
            }
        }
        public event Action? GridUpdated;
        public event Action<int> ClickEdit;
        public void invokeClickEdit(int id)
        {
            ClickEdit.Invoke(id);
        }

        public GridViewModel(UcRoot root)
        {
            _root = root;
            ReloadGrid();
        }

        public void MoveBin(DTOBin bin, int newX, int newY)
        {
            _root.Grid.moveBinInGrid(Grid, bin, newX, newY);
            GridUpdated?.Invoke();
        }
        public bool isAreaFree(int x, int y, DTOBin bin)
        {
            return _root.Grid.isBinMovePossible(Grid, bin, x, y);
        }
        public void ReloadGrid()
        {
            if (SelectedGrid == null) return;
            Grid = _root.Grid.getGridByID((int)SelectedGrid);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
