using Backend.Application.DTOs;
using InvenfinityApp.ViewModel;
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

namespace InvenfinityApp.Views
{
    /// <summary>
    /// Interaktionslogik für Grid.xaml
    /// </summary>
    public partial class ViewGrid : Page
    {

        private ViewGridViewModel data;
        public ViewGrid()
        {
            InitializeComponent();
            data = new ViewGridViewModel();
            DataContext = data;

            Loaded += ViewGrid_Loaded;
        }








        private void ViewGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewGridViewModel vm)
            {
                BuildGrid(vm.Grid);
            }
        }

        private void BuildGrid(DTOGrid gridDto)
        {
            var grid = FindVisualChild<Grid>(this, "DrawGrid");
            if (grid == null) return;

            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < gridDto.WidthCells; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < gridDto.HeightCells; i++)
                grid.RowDefinitions.Add(new RowDefinition());
        }

        #region Drag & Drop


    }
}
