using Backend.Application.DTOs;
using InvenfinityApp.ViewModel;
using InvenfinityApp.ViewModel.Grid;
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
    public partial class GridBoard : UserControl
    {
        private GridViewModel? VM =>
            DataContext as GridViewModel;
        public GridBoard()
        {
            InitializeComponent();
            Loaded += GridBoardControl_Loaded;
        }

        





        private void BuildGrid(DTOGrid gridDto)
        {
            DrawGrid.RowDefinitions.Clear();
            DrawGrid.ColumnDefinitions.Clear();
            DrawGrid.Children.Clear();

            for (int i = 0; i < gridDto.WidthCells; i++)
                DrawGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < gridDto.HeightCells; i++)
                DrawGrid.RowDefinitions.Add(new RowDefinition());

            // Bins hinzufügen
            foreach (var bin in gridDto.Bins)
            {
                var binControl = new GridBin { DataContext = bin };
                if (VM != null)
                    binControl.ClickEdit += VM.invokeClickEdit;
                Grid.SetColumn(binControl, bin.X);
                Grid.SetRow(binControl, bin.Y);
                Grid.SetColumnSpan(binControl, bin.WidthCells);
                Grid.SetRowSpan(binControl, bin.HeightCells);
                Panel.SetZIndex(binControl, bin.ZIndex);

                DrawGrid.Children.Add(binControl);
            }
        }

        private void GridBoardControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM != null)
            {
                BuildGrid(VM.Grid);

                // Auf PropertyChanged hören
                VM.PropertyChanged += VM_PropertyChanged;
            }
        }

        private void VM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GridViewModel.Grid))
            {
                // Grid neu zeichnen
                BuildGrid(VM!.Grid);
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(DTOBin)) || VM == null)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            var bin = (DTOBin)e.Data.GetData(typeof(DTOBin));

            var position = e.GetPosition(DrawGrid);

            int newX = GetColumnFromPosition(DrawGrid, position.X);
            int newY = GetRowFromPosition(DrawGrid, position.Y);

            e.Effects = VM.isAreaFree(newX, newY, bin)
                ? DragDropEffects.Move
                : DragDropEffects.None;

            e.Handled = true;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(DTOBin)) || VM == null)
                return;

            var bin = (DTOBin)e.Data.GetData(typeof(DTOBin));

            var position = e.GetPosition(DrawGrid);

            int newX = GetColumnFromPosition(DrawGrid, position.X);
            int newY = GetRowFromPosition(DrawGrid, position.Y);

            if (!VM.isAreaFree(newX, newY, bin))
                return;

            VM.MoveBin(bin, newX, newY);
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


    }
}
