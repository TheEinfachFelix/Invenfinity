using InvenfinityApp.Pages;
using InvenfinityApp.ViewModel;
using InvenfinityApp.ViewModel.Grid;
using InvenfinityApp.ViewModel.Tree;
using InvenfinityApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvenfinityApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel vm;
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            this.vm = vm;


            GridFrame.Content = new PageGrid(vm.GridVM, vm.LocationTreeVM)
            {
                DataContext = vm.GridVM
            };
            LocationEdit.Content = new PageLocationTreeEdit(vm.LocationEditVM, vm.CreateLocationVM, vm.CreateGridVM, vm.LocationTreeVM)
            {
                DataContext = vm.LocationEditVM
            };
            BinEdit.Content = new PageBinEdit(vm.BinEditVM, vm.CreateBinVM, vm.CreateBinTypeVM)
            {
                DataContext = vm.BinEditVM
            };
        }

        public void OpenBinToEdit(int binID)
        {
            vm.BinEditVM.SelectedBinId = binID;
            MainTabControl.SelectedIndex = 2;
        }
    }
}
