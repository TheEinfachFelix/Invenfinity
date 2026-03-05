using InvenfinityApp.ViewModel.Grid;
using InvenfinityApp.ViewModel.Part;
using InvenfinityApp.ViewModel.Tree;

namespace InvenfinityApp.ViewModel
{
    public class MainViewModel
    {
        public GridViewModel GridVM { get; }
        public LocationEditViewModel LocationEditVM { get; }
        public CreateGridViewModel CreateGridVM { get; }
        public CreateLocationViewModel CreateLocationVM { get; }
        public LocationTreeViewModel LocationTreeVM { get; }
        public CreateBinTypeViewModel CreateBinTypeVM { get; }
        public CreateBinViewModel CreateBinVM { get; }
        public CreatePartViewModel CreatePartVM { get; }
        public EditBinViewModel BinEditVM { get; }

        public MainViewModel(
            GridViewModel gridVm,
            LocationEditViewModel locEditVm,
            CreateGridViewModel createGridVm,
            CreateLocationViewModel createLocationVm,
            LocationTreeViewModel locationTreeViewModel,
            CreateBinTypeViewModel createBinTypeVM,
            CreateBinViewModel createBinVM,
            CreatePartViewModel createPartVM,
            EditBinViewModel binEditVM)
        {
            GridVM = gridVm;
            LocationEditVM = locEditVm;
            CreateGridVM = createGridVm;
            CreateLocationVM = createLocationVm;
            LocationTreeVM = locationTreeViewModel;
            CreateBinTypeVM = createBinTypeVM;
            CreateBinVM = createBinVM;
            CreatePartVM = createPartVM;
            BinEditVM = binEditVM;

            CreateGridVM.GridCreated += ReloadAll;
            CreateLocationVM.LocationCreated += ReloadAll;
            LocationEditVM.TreeChanged += ReloadAll;
            GridVM.GridUpdated += ReloadAll;
            CreateBinTypeVM.BinTypeChanged += ReloadBinTypes;
            CreateBinVM.BinsChanged += ReloadGrids;
            
        }

        private void ReloadAll()
        {
            ReloadGrids();
            LocationTreeVM.ReloadTree();
        }

        public void ReloadGrids()
        {
            GridVM.ReloadGrid();
            CreateBinVM.ReloadGrids();
            BinEditVM.ReloadGrids();
            BinEditVM.ReloadBins();
        }
        public void ReloadBinTypes()
        {
            CreateBinVM.ReloadBinTypes();
        }
    }
}
