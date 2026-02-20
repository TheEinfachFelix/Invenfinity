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
using Backend.Application.DTOs;

namespace InvenfinityApp.Views
{
    /// <summary>
    /// Interaktionslogik für Grid.xaml
    /// </summary>
    public partial class ViewGrid : Page
    {
        public ViewGrid()
        {
            InitializeComponent();
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is not DTOGrid)
            {
                // Selektion zurücksetzen
                var tree = (TreeView)sender;
                tree.SelectedItemChanged -= Tree_SelectedItemChanged;
                ((TreeViewItem)tree.ItemContainerGenerator.ContainerFromItem(e.NewValue))?.IsSelected = false;
                tree.SelectedItemChanged += Tree_SelectedItemChanged;
            }
        }
    }
}
