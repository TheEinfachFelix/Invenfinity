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
        public MainWindow(MainViewModel vm, GridViewModel gridVm, LocationEditViewModel locEdit, CreateLocationViewModel createLoc, CreateGridViewModel createGrid)
        {
            InitializeComponent();
            DataContext = vm;


            GridFrame.Content = new ViewGrid(gridVm)
            {
                DataContext = gridVm
            };
            LocationEdit.Content = new PageLocationTreeEdit(locEdit, createLoc, createGrid)
            {
                DataContext =locEdit
            };
        }
    }
}
