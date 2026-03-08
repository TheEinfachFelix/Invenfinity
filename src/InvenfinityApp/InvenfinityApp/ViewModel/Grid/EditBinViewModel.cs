using Backend.Application.DTOs.Grid;
using Backend.Application.DTOs.Location;
using Backend.Application.UseCases;
using Backend.Exceptions;
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

        private int? _selectedGridId;
        public int? SelectedGridID
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

        ///////////////////// Parts
        private int _selectedSlotIndex;
        public int SelectedSlotIndex
        {
            get => _selectedSlotIndex;
            set
            {
                _selectedSlotIndex = value;
                OnPropertyChanged(nameof(SelectedSlotIndex));
            }
        }
        private string _partSearchText = "";
        public string PartSearchText
        {
            get => _partSearchText;
            set
            {
                if (_partSearchText != value)
                {
                    _partSearchText = value;
                    OnPropertyChanged(nameof(PartSearchText));
                    UpdateSearch();
                }
            }
        }
        private ObservableCollection<IDtoPart> _availableParts = new();
        public ObservableCollection<IDtoPart> AvailableParts
        {
            get => _availableParts;
            set
            {
                _availableParts = value;
                OnPropertyChanged(nameof(AvailableParts));
            }
        }
        private ObservableCollection<IDtoPart> _searchResults = new();
        public ObservableCollection<IDtoPart> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
            }
        }
        public EditBinViewModel(UcRoot root)
        {
            this.root = root;
            _selectedGridlessBinId = 1;
            _gridlessBins = new();
            _binTypeName = string.Empty;
            _selectedGridId = null;
            _binTypeName = string.Empty;
            _Grids = new();
            _parts = new();
            UpdateProps();
            ReloadGrids();
            ReloadBins();
            LoadParts();
        }
        public event Action? BinsChanged;

        public void UpdateProps()
        {
            var bin = root.Bins.GetBinById(SelectedBinId);
            if (bin == null) return;
            BinTypeName = bin.BinType.Name;
            var grid = bin.GridId;
            SelectedGridID = grid;
            Parts = new(bin.Parts);
            DelPossible = bin.isDeletable;
            CheckMoveToGridPossigble();
        }

        public void ReloadGrids()
        {
            Grids = new(root.Bins.GetAllGrids());
        }

        public void ReloadBins()
        {
            GridlessBins = new(root.Bins.GetGridlessBins());
        }

        public void TakeGridlessBin()
        {
            SelectedBinId = SelectedGridlessBinId;
            UpdateProps();
        }

        public void CheckMoveToGridPossigble()
        {
            var bin = root.Bins.GetBinById(SelectedBinId) ?? throw new NotFoundException("Bin", SelectedBinId);
            UpdatePossible = root.Bins.CanCreateBin(bin.BinType.Id, SelectedGridID, SelectedBinId);
        }

        public void deleteBin()
        {
            var bin = root.Bins.GetBinById(SelectedBinId);
            if (bin == null || !bin.isDeletable) return;
            root.Bins.DeleteBin(SelectedBinId);
            BinsChanged?.Invoke();
        }

        public void UpdateBin()
        {
            if (!UpdatePossible) return;
            root.Bins.UpdateBin(SelectedBinId, [.. Parts], SelectedGridID);
            BinsChanged?.Invoke();
        }

        // Parts

        private void UpdateSearch()
        {
            SearchResults.Clear();

            if (string.IsNullOrWhiteSpace(PartSearchText))
                return;

            foreach (var part in AvailableParts)
            {
                if (part.Name.Contains(PartSearchText, StringComparison.OrdinalIgnoreCase))
                    SearchResults.Add(part);
            }
        }

        public void MovePartUp(IDtoPart part)
        {
            int index = Parts.IndexOf(part);
            if (index <= 0) return;

            (Parts[index - 1], Parts[index]) =
            (Parts[index], Parts[index - 1]);

            OnPropertyChanged(nameof(Parts));
        }

        public void MovePartDown(IDtoPart part)
        {
            int index = Parts.IndexOf(part);
            if (index < 0 || index >= Parts.Count - 1) return;

            (Parts[index + 1], Parts[index]) =
            (Parts[index], Parts[index + 1]);

            OnPropertyChanged(nameof(Parts));
        }

        public void RemovePart(IDtoPart part)
        {
            int index = Parts.IndexOf(part);
            if (index < 0) return;

            Parts[index] = DTOPartEmpty.Instance;
            OnPropertyChanged(nameof(Parts));
        }

        public void AddPart(IDtoPart part)
        {
            int index = Parts.IndexOf(DTOPartEmpty.Instance);

            if (index < 0)
                index = 0;

            Parts[index] = part;

            OnPropertyChanged(nameof(Parts));
        }
        private void LoadParts()
        {
            AvailableParts = new(root.Bins.GetAllParts());
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
