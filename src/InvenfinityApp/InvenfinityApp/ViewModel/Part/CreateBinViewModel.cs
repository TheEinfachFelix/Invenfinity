using Backend.Application.DTOs;
using Backend.Application.DTOs.Grid.Edit;
using Backend.Application.DTOs.Location;
using Backend.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace InvenfinityApp.ViewModel.Part
{
    public class CreateBinViewModel : INotifyPropertyChanged
    {
        private UcRoot root;
        private ObservableCollection<DTOEditBinType> _BinTypes;
        public ObservableCollection<DTOEditBinType> BinTypes
        {
            get => _BinTypes;
            set
            {
                _BinTypes = value;
                OnPropertyChanged(nameof(BinTypes));
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

        private int _SelectedBinTypeId;
        public int SelectedBinTypeId
        {
            get => _SelectedBinTypeId;
            set
            {
                _SelectedBinTypeId = value;
                OnPropertyChanged(nameof(SelectedBinTypeId));
            }
        }

        private int _SelectedGridId;
        public int SelectedGridId
        {
            get => _SelectedGridId;
            set
            {
                _SelectedGridId = value;
                OnPropertyChanged(nameof(SelectedGridId));
            }
        }

        public CreateBinViewModel(UcRoot root)
        {
            this.root = root;
            _BinTypes = new ObservableCollection<DTOEditBinType>();
            _Grids = new ObservableCollection<DTOTreeGrid>();
            ReloadBinTypes();
            ReloadGrids();

        }
        public event Action? BinsChanged;
        public void CreateBin()
        {
            root.Bin.CreateBin(SelectedBinTypeId, SelectedGridId);
            BinsChanged?.Invoke();
        }

        public void ReloadGrids()
        {
            _Grids = new ObservableCollection<DTOTreeGrid>();
            foreach (var grid in root.Bin.GetAllGrids())
            {
                _Grids.Add(grid);
            }
        }

        public void ReloadBinTypes()
        {
            _BinTypes = new ObservableCollection<DTOEditBinType>();
            foreach (var binType in root.Bin.GetAllBinTypes())
            {
                _BinTypes.Add(binType);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
