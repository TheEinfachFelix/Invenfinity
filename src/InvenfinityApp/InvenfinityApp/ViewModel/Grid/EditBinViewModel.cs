using Backend.Application.DTOs.Grid;
using Backend.Application.DTOs.Location;
using Backend.Application.UseCases;
using DBconnector.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace InvenfinityApp.ViewModel.Grid
{
    public class EditBinViewModel : INotifyPropertyChanged
    {
        private UcRoot root;
        private int _selectedGridlessBinId;
        public int SelectedGridlessBinId
        {             
            get { return _selectedGridlessBinId; }
            set
            {
                if (_selectedGridlessBinId != value)
                {
                    _selectedGridlessBinId = value;
                    OnPropertyChanged(nameof(SelectedGridlessBinId));
                }
            }
        }
        private int _selectedBinId;
        public int SelectedBinId
        {
            get { return _selectedBinId; }
            set
            {
                if (_selectedBinId != value)
                {
                    _selectedBinId = value;
                    OnPropertyChanged(nameof(SelectedBinId));
                    UpdateProps();
                }
            }
        }

        private ObservableCollection<DTOBin> _gridlessBins;
        public ObservableCollection<DTOBin> GridlessBins
        {
            get => _gridlessBins;
            set
            {
                _gridlessBins = value;
                OnPropertyChanged(nameof(GridlessBins));
            }
        }

        private string _binTypeName;
        public string BinTypeName
        {
            get { return _binTypeName; }
            set
            {
                if (_binTypeName != value)
                {
                    _binTypeName = value;
                    OnPropertyChanged(nameof(BinTypeName));
                }
            }
        }

        private int _selectedGridId;
        public int SelectedGridID
        {
            get { return _selectedGridId; }
            set
            {
                if (_selectedGridId != value)
                {
                    _selectedGridId = value;
                    OnPropertyChanged(nameof(SelectedGridID));
                    CheckMoveToGridPossigble();
                }
            }
        }
        private bool _updatePossible;
        public bool UpdatePossible
        {
            get => _updatePossible;
            set
            {
                _updatePossible = value;
                OnPropertyChanged(nameof(UpdatePossible));
            }
        }
        private ObservableCollection<DTOTreeGrid> _Grids;
        public ObservableCollection<DTOTreeGrid> Grids
        {
            get => _Grids;
            set
            {
                _Grids = value;
                OnPropertyChanged(nameof(Grids));
            }
        }
        private ObservableCollection<IDtoPart> _parts;
        public ObservableCollection<IDtoPart> Parts
        {
            get => _parts;
            set
            {
                _parts = value;
                OnPropertyChanged(nameof(Parts));
            }
        }
        private bool _delPossible;
        public bool DelPossible
        {
            get => _delPossible;
            set
            {
                _delPossible = value;
                OnPropertyChanged(nameof(DelPossible));
            }
        }
        public EditBinViewModel(UcRoot root)
        {
            this.root = root;
            _selectedGridlessBinId = 1;
            _gridlessBins = new();
            _binTypeName = string.Empty;
            _selectedGridId = -1;
            _binTypeName = string.Empty;
            _Grids = new();
            _parts = new();
            UpdateProps();
            ReloadGrids();
            ReloadBins();
        }
        public event Action? BinsChanged;

        public void UpdateProps()
        {
            var bin = root.Bin.GetBinById(SelectedBinId);
            if (bin == null) return;
            BinTypeName = bin.BinType.Name;
            var grid = bin.GridId;
            if (grid == null) SelectedGridID = -1;
            else
                SelectedGridID = (int)grid;
            Parts = new(bin.Parts);
            DelPossible = bin.isDeletable;
            CheckMoveToGridPossigble();
        }

        public void ReloadGrids()
        {
            Grids = new(root.Bin.GetAllGrids());
        }

        public void ReloadBins()
        {
            GridlessBins = new(root.BinEdit.getGridlessBins());
        }

        public void TakeGridlessBin()
        {
            SelectedBinId = SelectedGridlessBinId;
            UpdateProps();
        }

        public void CheckMoveToGridPossigble()
        {
            var bin = root.Bin.GetBinById(SelectedBinId);
            int? gridID = null;
            if (SelectedGridID != -1)
                gridID = SelectedGridID;
            UpdatePossible = root.Bin.CanCreateBin(bin.BinType.Id, gridID);
        }

        public void deleteBin()
        {
            if (!root.Bin.GetBinById(SelectedBinId).isDeletable) return;
            root.BinEdit.deleteBin(SelectedBinId);
            BinsChanged?.Invoke();
        }

        public void UpdateBin()
        {
            if (!UpdatePossible) return;
            int? gridID = null;
            if (SelectedGridID != -1)
                gridID = SelectedGridID;
            root.BinEdit.updateBin(SelectedBinId, [.. Parts], gridID);
            BinsChanged?.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
