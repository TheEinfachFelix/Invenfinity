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
using System.Windows.Shapes;

namespace InvenfinityApp.Windows
{
    /// <summary>
    /// Interaktionslogik für CreateLocation.xaml
    /// </summary>
    public partial class CreateGrid : Window
    {
        public CreateGrid()
        {
            InitializeComponent();
            DataContext = Global.ViewGridViewModel;
            locTree.SelectionChanged += locTree_SelectionChanged;
            CreateBtn.IsEnabled = false;
        }

        private void locTree_SelectionChanged(object? sender, EventArgs e)
        {
            if (locTree.SelectedLocationId != null)
            {
                ParentVal.Text = locTree.SelectedLocationId.ToString();
                CreateBtn.IsEnabled = true;
            }
            else
            {
                ParentVal.Text = "";
                CreateBtn.IsEnabled = false;
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (NameVal.Text != "" && int.TryParse(ParentVal.Text, out int parentID) && int.TryParse(XsizeVal.Text, out int Xsize) && int.TryParse(YsizeVal.Text, out int Ysize))
            {
                Global.UcRoot.Locations.Edit.CreateGrid(NameVal.Text, parentID, Xsize, Ysize);
                Global.ViewGridViewModel.ReloadGrid();
                this.Close();
            }
        }
    }
}
