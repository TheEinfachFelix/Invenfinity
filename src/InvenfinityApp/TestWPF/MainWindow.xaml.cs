using LabelMaker;
using LabelMaker.Models.Bin;
using LabelMaker.Models.Part;
using LabelMakerWPF.Templates.Printer;
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

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var Part = new PartDataModel()
            {
                Thread = ScrewThreadType.M3,
                Length = 8,
                Typename = "mechanical_screw_cylinder"
            };
            var Part2 = new PartDataModel()
            {
                Thread = ScrewThreadType.M6,
                Length = 25,
                Typename = "mechanical_screw_cylinder"
            };
            var Bin = new BinDataModel()
            {
                UnitLength = 2,
                SlotCount = 1,
                Parts = [Part]
            };

            var assetPath = "C:/Github/Invenfinity/src/Assets/";

            var drawing = Bin.ToLabel(assetPath);

            //img.Source = var.PreviewBin(Bin);
            img.Source = drawing.RenderImg(12);

            //img.Source = var.Print(Bin, new PrinterPTouchP700(), false);
        }
    }
}