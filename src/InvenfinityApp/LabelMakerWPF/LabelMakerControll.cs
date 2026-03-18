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
using System.Reflection;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabelMaker
{
    public static class LabelMakerControll
    {

        public static BinLabelRoot ToLabel(this BinDataModel bin, string AssetPath)
        {
            var data = JsonTemplateLoader.LoadJson(bin.GetTemplatePath(AssetPath));
            if (data.version != 1) throw new Exception("Invallide Json Version");
            return Converter.ToLabel(AssetPath, data, bin);
        }
        public static DrawingGroup Render(this BinLabelRoot label, double Heigthmm)
        {
            double heightUnits = Converter.mmtoUnits(Heigthmm);
            var data = label.Render(heightUnits);
            Trace.WriteLine(label.labelLength);
            Trace.WriteLine(Converter.UnitsToMm(data.Bounds.Width));
            Trace.WriteLine(Converter.UnitsToMm(data.Bounds.Height));
            return data;
        }
        public static DrawingImage RenderImg(this BinLabelRoot label, double Heigthmm)
        {
            return new (Render(label, Heigthmm));
        }

        public static void Print(this BinLabelRoot label, IPrinter printer, bool showDialog)
        {
            var drawing = label.Render(printer.MaxYSize);
            new LabelRenderEngine().PrintVekor(drawing, printer, showDialog);
        }
    }
}
