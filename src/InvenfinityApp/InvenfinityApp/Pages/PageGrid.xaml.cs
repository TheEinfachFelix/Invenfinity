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

        public ViewGrid()
        {
            InitializeComponent();
            DataContext = Global.ViewGridViewModel;

        }

        
        private void EditLocations_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainTabControl.SelectedIndex = 1;
            }
        }

    }
}
