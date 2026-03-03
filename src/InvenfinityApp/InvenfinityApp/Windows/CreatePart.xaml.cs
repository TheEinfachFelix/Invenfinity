using InvenfinityApp.ViewModel.Part;
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
    /// Interaktionslogik für CreatePart.xaml
    /// </summary>
    public partial class CreatePart : Window
    {
        private CreatePartViewModel vm;
        public CreatePart(CreatePartViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(InventreeIdVal.Text, out int invtreeID))
            {
                vm.CreatePart(invtreeID);
                this.Close();
            }
        }
    }
}
