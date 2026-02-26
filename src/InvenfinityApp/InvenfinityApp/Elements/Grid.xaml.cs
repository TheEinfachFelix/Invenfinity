using InvenfinityApp.ViewModel;
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

namespace InvenfinityApp.Elements
{
    /// <summary>
    /// Interaktionslogik für Grid.xaml
    /// </summary>
    public partial class Grid : UserControl
    {
        public ViewGridViewModel data { get; set; }

        public Grid()
        {
            InitializeComponent();
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

            if (data.isAreaFree(newX, newY, _draggedBin))
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

            if (!data.isAreaFree(newX, newY, _draggedBin))
                return;

            vm.MoveBin(_draggedBin, newX, newY);

            _draggedBin = null;
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
