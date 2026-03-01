using Backend.Application.DTOs;
using InvenfinityApp.ViewModel;
using InvenfinityApp.ViewModel.Tree;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
    /// Interaktionslogik für PageLocationTreeEdit.xaml
    /// </summary>
    public partial class PageLocationTreeEdit : Page
    {
        private CreateLocationViewModel CreateLocViewModel;
        private CreateGridViewModel CreateGridViewModel;
        private readonly LocationEditViewModel vm;

        public PageLocationTreeEdit(LocationEditViewModel vm, CreateLocationViewModel CreateLocViewModel, CreateGridViewModel CreateGridViewModel)
        {
            InitializeComponent();
            DataContext = vm;
            this.vm = vm;
            this.CreateLocViewModel = CreateLocViewModel;
            this.CreateGridViewModel = CreateGridViewModel;
            LocationTree.SelectionChanged += LocationTree_SelectionChanged;
        }

        private void LocationTree_SelectionChanged(object sender, EventArgs e)
        {
            var item = LocationTree.SelectedTreeItem;
            if (item == null) return;
            vm.UpdateItemEdit(item);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            vm.SaveItemEdit();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteItemEdit();
        }

        private void NewLoc_Click(object sender, RoutedEventArgs e)
        {
            var createLocationWindow = new Windows.CreateLocation(CreateLocViewModel);
            createLocationWindow.ShowDialog();
        }

        private void NewGrid_Click(object sender, RoutedEventArgs e)
        {
            var createGridWindow = new Windows.CreateGrid(CreateGridViewModel);
            createGridWindow.ShowDialog();
        }


    }
}
