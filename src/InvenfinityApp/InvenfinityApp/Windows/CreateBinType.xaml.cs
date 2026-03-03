using InvenfinityApp.ViewModel.Grid;
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
    /// Interaktionslogik für CreateBinType.xaml
    /// </summary>
    public partial class CreateBinType : Window
    {
        private CreateBinTypeViewModel vm;
        public CreateBinType(CreateBinTypeViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Slotcnt.Text, out int SlotCount) && int.TryParse(Xsize.Text, out int X) && int.TryParse(Ysize.Text, out int Y))
            {
                if (SlotCount < 1 && X < 1 && Y < 1) return;
                vm.CreateBinType(SlotCount,X, Y);
                this.Close();
            }
        }
    }
}
