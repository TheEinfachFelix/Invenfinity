using LabelMaker;
using LabelMaker.Models.Bin;
using LabelMaker.Models.Part;
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

            var Bin = new BinDataModel()
            {
                UnitLength = 6,
                SlotCount = 1,
            };
            var Part = new PartDataModel()
            {
                Thread = ScrewThreadType.M6,
                Length = 25,
                Head = ScrewHeadType.Senkkopf,
                Drive = ScrewDriveType.Philips
            };
            var path = "C:\\Github\\Invenfinity\\template.json";
            var assetPath = "C:/Github/Invenfinity/src/Assets/";
            LabelMakerControll var = new(assetPath);
            img.Source = var.test(path, Bin, Part);
        }
    }
}