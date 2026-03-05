using Backend.Application.DTOs;
using Backend.Application.DTOs.Grid.Edit;
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

        private ObservableCollection<DTOEditBin> _Bins;
        public ObservableCollection<DTOEditBin> Bins
        {
            get => _Bins;
            set
            {
                _Bins = value;
                OnPropertyChanged(nameof(Bins));
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
                }
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
        private ObservableCollection<DTOEditPart> _parts;
        public ObservableCollection<DTOEditPart> Parts
        {
            get => _parts;
            set
            {
                _parts = value;
                OnPropertyChanged(nameof(Parts));
            }
        }
        public EditBinViewModel(UcRoot root)
        {
            this.root = root;
            _selectedBinId = 1;
            _Bins = new();
            _binTypeName = string.Empty;
            _selectedGridId = -1;
            _binTypeName = string.Empty;
            _Grids = new();
            _parts = new();
            UpdateProps();

        }

        public void UpdateProps()
        {
            var bin = root.Bin.GetBinById(SelectedBinId);
            BinTypeName = bin.BinType.Name;
            var grid = bin.GridId;
            if (grid == null) SelectedGridID = -1;
            else
                SelectedGridID = (int)grid;
            Parts = new(bin.Parts);
            ReloadGrids();
            ReloadBins();
        }

        public void ReloadGrids()
        {
            Grids = new(root.Bin.GetAllGrids());
        }

        public void ReloadBins()
        {
            Bins = new(root.BinEdit.getBins());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
