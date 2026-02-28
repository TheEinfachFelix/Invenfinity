using Backend.Application.DTOs;
using InvenfinityApp.ViewModel;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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

namespace InvenfinityApp.Pages
{
    /// <summary>
    /// Interaktionslogik für PageLocationTreeEdit.xaml
    /// </summary>
    public partial class PageLocationTreeEdit : Page
    {
        private readonly ViewGridViewModel _vm;
        private IDotTreeItem? selectedItem;

        public PageLocationTreeEdit()
        {
            InitializeComponent();
            _vm = Global.ViewGridViewModel;
            DataContext = _vm; 
            LocationTree.SelectionChanged += LocationTree_SelectionChanged;
        }

        private void LocationTree_SelectionChanged(object sender, EventArgs e)
        {
            var item = LocationTree.SelectedTreeItem;
            if (item == null) return;
            _vm.UpdateItemEdit(item);
        }



        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }



    }
}
