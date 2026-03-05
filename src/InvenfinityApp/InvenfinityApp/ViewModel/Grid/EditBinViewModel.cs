using Backend.Application.DTOs;
using Backend.Application.DTOs.Grid.Edit;
using Backend.Application.UseCases;
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
        private int _binID;
        public int BinID
        {             
            get { return _binID; }
            set
            {
                if (_binID != value)
                {
                    _binID = value;
                    OnPropertyChanged(nameof(BinID));
                    UpdateProps();
                }
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

        private int _gridId;
        public int GridID
        {
            get { return _gridId; }
            set
            {
                if (_gridId != value)
                {
                    _gridId = value;
                    OnPropertyChanged(nameof(GridID));
                }
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
            _binID = 1;
            _binTypeName = string.Empty;
            _gridId = -1;
            _binTypeName = string.Empty;
            _parts = new ObservableCollection<DTOEditPart>();
            UpdateProps();
        }

        public void UpdateProps()
        {
            var bin = root.Bin.GetBinById(BinID);
            BinTypeName = bin.BinType.Name;
            var grid = bin.GridId;
            if (grid == null) GridID = -1;
            else
                GridID = (int)grid;
            Parts = new ObservableCollection<DTOEditPart>(bin.Parts);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
