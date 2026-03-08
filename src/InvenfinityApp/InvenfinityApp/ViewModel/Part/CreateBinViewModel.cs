using Backend.Application.DTOs;
using Backend.Application.DTOs.Grid;
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
        private ObservableCollection<DTOBinType> _BinTypes;
        public ObservableCollection<DTOBinType> BinTypes
        {
            get => _BinTypes;
            set
            {
                _BinTypes = value;
                OnPropertyChanged(nameof(BinTypes));
            }
        }
        private ObservableCollection<Backend.Application.DTOs.DTOTreeGrid> _Grids;
        public ObservableCollection<Backend.Application.DTOs.DTOTreeGrid> Grids
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
                CheckCanCreate();
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
                CheckCanCreate();
            }
        }
        private bool _canCreate;
        public bool canCreate
        {
            get => _canCreate;
            set
            {
                _canCreate = value;
                OnPropertyChanged(nameof(canCreate));
            }
        }

        public CreateBinViewModel(UcRoot root)
        {
            this.root = root;
            _BinTypes = new ObservableCollection<DTOBinType>();
            _Grids = new ObservableCollection<Backend.Application.DTOs.DTOTreeGrid>();
            ReloadBinTypes();
            ReloadGrids();

        }
        public event Action? BinsChanged;
        public void CreateBin()
        {
            int? gridID = null;
            if (SelectedGridId != -1)
                gridID = SelectedGridId;
            root.Bin.CreateBin(SelectedBinTypeId, gridID);
            BinsChanged?.Invoke();
        }

        public void ReloadGrids()
        {
            Grids = new ObservableCollection<Backend.Application.DTOs.DTOTreeGrid>((List<Backend.Application.DTOs.DTOTreeGrid>)root.Bin.GetAllGrids());
            CheckCanCreate();
        }

        public void ReloadBinTypes()
        {
            BinTypes = new ObservableCollection<DTOBinType>(root.Bin.GetAllBinTypes());
            CheckCanCreate();
        }

        public void CheckCanCreate()
        {
            int? gridID = null;
            if (SelectedGridId != -1)
                gridID = SelectedGridId;
            if (SelectedBinTypeId == 0)
            {
                canCreate = false;
                return;
            }
            canCreate = root.Bin.CanCreateBin(SelectedBinTypeId, gridID);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
