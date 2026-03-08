using InvenfinityApp.ViewModel.Grid;
using InvenfinityApp.ViewModel.Part;
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
using System.Windows.Shapes;

namespace InvenfinityApp.Windows
{
    /// <summary>
    /// Interaktionslogik für CreateBin.xaml
    /// </summary>
    public partial class CreateBin : Window
    {
        private CreateBinViewModel vm;
        private CreateBinTypeViewModel createBinTypeViewModel;
        public CreateBin(CreateBinViewModel vm, CreateBinTypeViewModel createBinTypeViewModel)
        {
            InitializeComponent();
            this.vm = vm;
            DataContext = vm;
            this.createBinTypeViewModel = createBinTypeViewModel;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedBinTypeId == 0 || vm.SelectedGridId == 0) return;

            vm.CreateBin();
            this.Close();
        }

        private void Create_BinType_Click(object sender, RoutedEventArgs e)
        {
            CreateBinType createBinTypeWindow = new CreateBinType(createBinTypeViewModel);
            createBinTypeWindow.ShowDialog();
        }
    }
}
