using Backend.Application.DTOs;
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
    /// Interaktionslogik für GridBin.xaml
    /// </summary>
    public partial class GridBin : UserControl
    {
        public GridBin()
        {
            InitializeComponent();
        }

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
    }
}
