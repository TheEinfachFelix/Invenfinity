using Backend.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static readonly DependencyProperty RootProperty =
            DependencyProperty.Register(
                nameof(Root),
                typeof(ObservableCollection<IDotTreeItem>),
                typeof(LocationTree),
                new PropertyMetadata(null));

        public ObservableCollection<IDotTreeItem> Root
        {
            get => (ObservableCollection<IDotTreeItem>)GetValue(RootProperty);
            set => SetValue(RootProperty, value);
        }

        public LocationTree()
        {
            InitializeComponent();
            //this.DataContext = this;
        }

        private void GroupView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedGridId = null;
            SelectedLocationId = null;

            if (e.NewValue is DTOTreeGrid grid)
            {
                SelectedGridId = grid.Id;
            }
            if (e.NewValue is DTOTreeLocation location)
            {
                SelectedLocationId = location.Id;
            }
        }

    }
}
