using InvenfinityApp.ViewModel.Grid;
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
    /// Interaktionslogik für CreateBin.xaml
    /// </summary>
    public partial class CreateBin : Window
    {
        private CreateBinViewModel vm;
        public CreateBin(CreateBinViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Slotcnt.Text, out int SlotCount) && int.TryParse(Xsize.Text, out int X) && int.TryParse(Ysize.Text, out int Y))
            {
                if (SlotCount < 1 && X < 1 && Y < 1) return;
                vm.CreateBinType(SlotCount, X, Y);
                this.Close();
            }
        }
    }
}
