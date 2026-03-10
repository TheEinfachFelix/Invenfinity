using LabelMaker.Models.Bin;
using LabelMaker.Models.Label;
using LabelMaker.Models.Part;
using LabelMaker.Rendering;
using LabelMaker.Services;
using LabelMaker.Templates.Json;
using LabelMakerWPF.Templates.Printer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabelMaker
{
    public class LabelMakerControll
    {
        public string AssetPath { get; set; }
        public LabelMakerControll(string assetPath) 
        { 
            this.AssetPath = assetPath;
        }

        public DrawingImage test(string path, BinDataModel bin, PartDataModel part)
        {
            var data = JsonTemplateLoader.LoadJson(path);

            LabelRoot label = Converter.ToLabel(AssetPath, data, bin, part);

            LabelMakerControll var = new(AssetPath);
            var outp = new DrawingImage(label.BuildVector(Converter.mmtoUnits(12)));

            Trace.WriteLine(bin.SlotLableLength);
            Trace.WriteLine(Converter.UnitsToMm(outp.Width));
            Trace.WriteLine(Converter.UnitsToMm(outp.Height));

            return outp;

        }
        public void TestPrint(string path, BinDataModel bin, PartDataModel part, IPrinter printer) 
        {
            var data = JsonTemplateLoader.LoadJson(path);

            LabelRoot label = Converter.ToLabel(AssetPath, data, bin, part);

            new LabelRenderEngine().Print(label, printer);
        }
    }
}
