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
    public class LabelMakerControll
    {
        public string AssetPath { get; set; }
        public LabelMakerControll(string assetPath) 
        { 
            this.AssetPath = assetPath;
        }
        internal LabelRoot getLabel(BinDataModel bin, PartDataModel part)
        {
            var data = JsonTemplateLoader.LoadJson(part.GetTemplatePath(AssetPath));
            return Converter.ToLabel(AssetPath, data, bin, part);
        }
        public DrawingGroup GetVektor(BinDataModel bin, PartDataModel part, double Heightmm)
        {
            var label = getLabel(bin, part);
            return GetVektor(label, Heightmm);
        }
        internal DrawingGroup GetVektor(LabelRoot label, double Heightmm)
        {
            double labelHeightUnits = Converter.mmtoUnits(Heightmm);
            return label.BuildVector(labelHeightUnits);
        }

        public DrawingGroup GetVektorForBin(BinDataModel bin, double Heightmm)
        {
            var mainGroup = new DrawingGroup();
            double currentXOffset = 0;

            foreach (var part in bin.Parts)
            {
                var label = getLabel(bin, part);
                var partVector = GetVektor(label, Heightmm);

                // 3. Transformation erstellen, um das Label hinter das vorherige zu schieben
                var transformGroup = new DrawingGroup();
                transformGroup.Transform = new TranslateTransform(currentXOffset, 0);
                transformGroup.Children.Add(partVector);

                // 4. Zur Hauptgruppe hinzufügen
                mainGroup.Children.Add(transformGroup);

                // 5. Offset für das nächste Label erhöhen
                // Hier nutzen wir die berechnete Länge des Labels
                currentXOffset += partVector.Bounds.Width + bin.Padding;
            }

            return mainGroup;
        }
        public DrawingImage PreviewPart(BinDataModel bin, PartDataModel part)
        {
            var data = GetVektor(bin, part, 12);
            var outp = new DrawingImage(data);

            Trace.WriteLine(bin.SlotLableLength);
            Trace.WriteLine(Converter.UnitsToMm(outp.Width));
            Trace.WriteLine(Converter.UnitsToMm(outp.Height));

            return outp;
        }

        public DrawingImage PreviewBin(BinDataModel bin)   
        {
            var data = GetVektorForBin(bin,12);
            var outp = new DrawingImage(data);

            Trace.WriteLine(bin.SlotLableLength*2);
            Trace.WriteLine(Converter.UnitsToMm(outp.Width));
            Trace.WriteLine(Converter.UnitsToMm(outp.Height));

            return outp;
        }
        public void Print(BinDataModel bin, IPrinter printer, bool showDialog) 
        {
            double labelHeightUnits = Converter.mmtoUnits(Math.Min(12, printer.MaxYSize));

            var vektor = GetVektorForBin(bin, labelHeightUnits);

            new LabelRenderEngine().PrintVekor(vektor, printer, showDialog);
        }
    }
}
