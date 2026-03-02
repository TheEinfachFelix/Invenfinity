using InvenfinityApp.ViewModel.Grid;
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

        public MainViewModel(
            GridViewModel gridVm,
            LocationEditViewModel locEditVm,
            CreateGridViewModel createGridVm,
            CreateLocationViewModel createLocationVm,
            LocationTreeViewModel locationTreeViewModel)
        {
            GridVM = gridVm;
            LocationEditVM = locEditVm;
            CreateGridVM = createGridVm;
            CreateLocationVM = createLocationVm;
            LocationTreeVM = locationTreeViewModel;

            CreateGridVM.GridCreated += ReloadAll;
            CreateLocationVM.LocationCreated += ReloadAll;
            LocationEditVM.TreeChanged += ReloadAll;
            GridVM.GridUpdated += ReloadAll;
        }

        private void ReloadAll()
        {
            GridVM.ReloadGrid();
            LocationTreeVM.ReloadTree();
        }
    }
}
