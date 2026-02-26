using Backend.Application.DTOs;
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
    public partial class GridBoard : UserControl
    {
        private ViewGridViewModel? VM =>
            DataContext as ViewGridViewModel;
        public GridBoard()
        {
            InitializeComponent();
            Loaded += GridBoardControl_Loaded;
        }


        private void GridBoardControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM != null)
            {
                BuildGrid(VM.Grid);
            }
        }

        private void BuildGrid(DTOGrid gridDto)
        {
            DrawGrid.RowDefinitions.Clear();
            DrawGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < gridDto.WidthCells; i++)
                DrawGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < gridDto.HeightCells; i++)
                DrawGrid.RowDefinitions.Add(new RowDefinition());
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
