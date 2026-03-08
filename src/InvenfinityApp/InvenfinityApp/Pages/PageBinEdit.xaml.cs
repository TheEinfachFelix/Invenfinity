using InvenfinityApp.ViewModel.Grid;
using InvenfinityApp.ViewModel.Part;
using InvenfinityApp.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvenfinityApp.Pages
{
    /// <summary>
    /// Interaktionslogik für PageGridEdit.xaml
    /// </summary>
    public partial class PageBinEdit : Page
    {
        private EditBinViewModel vm;
        private CreateBinViewModel createBinViewModel;
        private CreateBinTypeViewModel createBinTypeViewModel;
        public PageBinEdit(EditBinViewModel vm, CreateBinViewModel createBinViewModel,CreateBinTypeViewModel createBinTypeViewModel)
        {
            InitializeComponent();
            this.vm = vm;
            DataContext = vm;
            this.createBinViewModel = createBinViewModel;
            this.createBinTypeViewModel = createBinTypeViewModel;
        }

        public void Create_Bin_Click(object sender, RoutedEventArgs e)
        {
            CreateBin createBinWindow = new CreateBin(createBinViewModel, createBinTypeViewModel);
            createBinWindow.ShowDialog();
        }
        public void Load_Gridless_Bin_Click(object sender, RoutedEventArgs a)
        {
            vm.TakeGridlessBin();
        }
        public void Save_Bin_Click(Object sender, RoutedEventArgs e)
        {
            vm.UpdateBin();
            vm.UpdateProps();
            vm.ReloadBins();
        }

        public void Del_Bin_Click(object sender, RoutedEventArgs e)
        {
            vm.deleteBin();
            vm.UpdateProps();
            vm.ReloadBins();
        }
    }
}
