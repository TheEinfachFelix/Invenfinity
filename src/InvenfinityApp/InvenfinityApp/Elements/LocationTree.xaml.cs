using Backend.Application.DTOs;
using Backend.Application.DTOs.Location;
using InvenfinityApp.ViewModel.Tree;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaktionslogik für LocationTree.xaml
    /// </summary>
    public partial class LocationTree : UserControl
    {
        public int? SelectedGridId { get; private set; }
        public int? SelectedLocationId { get; private set; }
        public IDtoTreeItem? SelectedTreeItem { get; private set; }
        public event EventHandler? SelectionChanged;
        public LocationTree()
        {
            InitializeComponent();
        }

        private void GroupView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedGridId = null;
            SelectedLocationId = null;
            SelectedTreeItem = (IDtoTreeItem?)e.NewValue;

            if (e.NewValue is DTOTreeGrid grid)
            {
                SelectedGridId = grid.Id;
            }
            if (e.NewValue is DTOTreeLocation location)
            {
                SelectedLocationId = location.Id;
            }

            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
