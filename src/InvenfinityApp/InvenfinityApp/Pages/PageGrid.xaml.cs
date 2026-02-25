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
        private Point _dragStart;
        private DTOBin? _draggedBin;
        private ViewGridViewModel data;
        public ViewGrid()
        {
            InitializeComponent();
            data = new ViewGridViewModel();
            DataContext = data;

            Loaded += ViewGrid_Loaded;
        }


        private void GroupView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is DTOTreeGrid grid)
            {
                // Methode aufrufen
                HandleGridSelected(grid);
            }
        }

        private void HandleGridSelected(DTOTreeGrid grid)
        {
            // Deine Logik hier
            MessageBox.Show($"DTOGrid ausgewählt: {grid.Name}");
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

        private void Bin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border &&
                border.DataContext is DTOBin bin)
            {
                _dragStart = e.GetPosition(null);
                _draggedBin = bin;
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            if (_draggedBin == null)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            var grid = sender as Grid;
            var position = e.GetPosition(grid);

            int newX = GetColumnFromPosition(grid, position.X);
            int newY = GetRowFromPosition(grid, position.Y);

            if (IsAreaFree(newX, newY, _draggedBin))
                e.Effects = DragDropEffects.Move;
            else
                e.Effects = DragDropEffects.None;

            e.Handled = true;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (DataContext is not ViewGridViewModel vm)
                return;

            if (_draggedBin == null) return;

            var grid = sender as Grid;
            var position = e.GetPosition(grid);

            int newX = GetColumnFromPosition(grid, position.X);
            int newY = GetRowFromPosition(grid, position.Y);

            if (!IsAreaFree(newX, newY, _draggedBin))
                return;

            vm.MoveBin(_draggedBin, newX, newY);

            _draggedBin = null;
        }

        private void Bin_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                _draggedBin != null)
            {
                DragDrop.DoDragDrop((DependencyObject)sender,
                                    _draggedBin,
                                    DragDropEffects.Move);
            }
        }

        private void Bin_Drop(object sender, DragEventArgs e)
        {
            if (_draggedBin == null) return;

            var position = e.GetPosition((IInputElement)sender);
            var grid = FindVisualChild<Grid>(this, "DrawGrid");
            if (grid == null) return;

            double cellWidth = grid.ActualWidth / grid.ColumnDefinitions.Count;
            double cellHeight = grid.ActualHeight / grid.RowDefinitions.Count;

            int newX = (int)(position.X / cellWidth);
            int newY = (int)(position.Y / cellHeight);

            // DTO ist immutable → hier müsstest du
            // entweder:
            // 1. neues DTO erzeugen
            // 2. oder ViewModel-Command verwenden
            // 3. oder DTOBin mutable machen

            MessageBox.Show($"Neue Posxxxxxxxxxxxxxxxxxxxition: {newX}, {newY}");
        }

        private int GetColumnFromPosition(Grid grid, double x)
        {
            double cellWidth = grid.ActualWidth / grid.ColumnDefinitions.Count;
            return Math.Max(0,
                Math.Min(grid.ColumnDefinitions.Count - 1,
                (int)(x / cellWidth)));
        }

        private int GetRowFromPosition(Grid grid, double y)
        {
            double cellHeight = grid.ActualHeight / grid.RowDefinitions.Count;
            return Math.Max(0,
                Math.Min(grid.RowDefinitions.Count - 1,
                (int)(y / cellHeight)));
        }

        private bool IsAreaFree(int x, int y, DTOBin bin)
        {
            if (DataContext is not ViewGridViewModel vm)
                return false;

            // außerhalb des Grids?
            if (x + bin.WidthCells > vm.Grid.WidthCells)
                return false;

            if (y + bin.HeightCells > vm.Grid.HeightCells)
                return false;

            foreach (var other in vm.Grid.Bins)
            {
                if (other == bin)
                    continue;

                bool overlap =
                    x < other.X + other.WidthCells &&
                    x + bin.WidthCells > other.X &&
                    y < other.Y + other.HeightCells &&
                    y + bin.HeightCells > other.Y;

                if (overlap)
                    return false;
            }

            return true;
        }



        #endregion

        private T? FindVisualChild<T>(DependencyObject parent, string name)
            where T : FrameworkElement
        {
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);

                if (child is T typed && typed.Name == name)
                    return typed;

                var result = FindVisualChild<T>(child, name);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
