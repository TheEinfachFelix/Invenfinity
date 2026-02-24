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
        public ViewGrid()
        {
            InitializeComponent();
            DataContext = new ViewGridViewModel();
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

            MessageBox.Show($"Neue Position: {newX}, {newY}");
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
