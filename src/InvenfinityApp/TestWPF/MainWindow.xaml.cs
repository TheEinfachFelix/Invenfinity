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
                Thread = ScrewThreadType.M6,
                Length = 25,
                Head = ScrewHeadType.Senkkopf,
                Drive = ScrewDriveType.Philips
            };
            var Part2 = new PartDataModel()
            {
                Thread = ScrewThreadType.M6,
                Length = 25,
                Head = ScrewHeadType.Senkkopf,
                Drive = ScrewDriveType.Philips
            };
            var Bin = new BinDataModel()
            {
                UnitLength = 2,
                SlotCount = 3,
                Parts = [Part, Part2]
            };

            var assetPath = "C:/Github/Invenfinity/src/Assets/";
            LabelMakerControll var = new(assetPath);
            img.Source = var.PreviewBin(Bin);

            //var.Print(Bin, new PrinterPTouchP700(), true);
        }
    }
}