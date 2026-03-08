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
        public event Action<int> ClickEdit;
        private Point _dragStart;
        // Damit auf den Button geklickt werden kann
        private void Bin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _dragStart = e.GetPosition(null);
        }

        private void Bin_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            Point current = e.GetPosition(null);

            if (Math.Abs(current.X - _dragStart.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(current.Y - _dragStart.Y) < SystemParameters.MinimumVerticalDragDistance)
                return;

            if (DataContext is DTOBin bin)
            {
                DragDrop.DoDragDrop(this, bin, DragDropEffects.Move);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var data = DataContext as DTOBin;
            if (data == null) throw new Exception("Wrong Data Context Type");
            var id = data.Id;
            ClickEdit.Invoke(id);
        }
    }
}
