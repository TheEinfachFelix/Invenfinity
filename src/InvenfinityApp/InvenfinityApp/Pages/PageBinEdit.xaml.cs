using Backend.Application.DTOs.Grid;
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

        private void MovePartUp_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is IDtoPart part)
                vm.MovePartUp(part);
        }

        private void MovePartDown_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is IDtoPart part)
                vm.MovePartDown(part);
        }

        private void RemovePart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is IDtoPart part)
                vm.RemovePart(part);
        }

        private void AddPart_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox list && list.SelectedItem is IDtoPart part)
            {
                vm.AddPart(part);
            }
        }
    }
}
