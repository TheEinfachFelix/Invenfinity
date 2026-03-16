using LabelMaker.Models.Bin;
using LabelMaker.Models.Label;
using LabelMaker.Models.Label.Elements;
using LabelMaker.Models.Part;
using LabelMaker.Services;
using LabelMaker.Templates.Json;
using LabelMakerWPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMakerWPF.Models.Label.Elements
{
    internal class LabelElementGroupe : LabelElementBase, ILabelElement
    {
        public List<ILabelElement> elements = [];
        public static string Name => "groupe";

        public LabelElementGroupe(double? padding, double minScale, double maxScale, List<ILabelElement> elements) : base(padding, minScale, maxScale)
        {
            this.elements = elements;
        }

        public override double GetWidth(double labelHeight, double scale)
        {
            return getContent(labelHeight).Bounds.Width;
        }

        public DrawingGroup getContent(double labelHeight)
        {
            var group = new DrawingGroup();

            if (!elements.Any()) return group;

            // 1. Berechne die individuellen Skalierungswerte für die feste Länge
            //var scales = CalcScale(labelHeight);

            double xOffset = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                var element = elements[i];
                double currentScale = 1;//scales[i];

                // Rendern mit der berechneten Skalierung
                element.Render(group, xOffset, labelHeight, currentScale);

                // Offset erhöhen
                xOffset += element.GetWidth(labelHeight, currentScale);

                if (element.Padding.HasValue)
                    xOffset += Converter.mmtoUnits(element.Padding.Value);
            }

            return group;
        }

        public override void Render(DrawingGroup group, double x, double labelHeight, double scale)
        {
            double targetHeight = labelHeight * scale;
            double targetWidth = GetWidth(labelHeight, scale);
            double y = CalculateYOffset(labelHeight, targetHeight);

            SvgHelper.DrawSvg(group, getContent(labelHeight), x, y, targetWidth, targetHeight);
        }
        public static LabelElementGroupe GenerateElement(LayoutItem item, BinDataModel bin, PartDataModel part, string assetPath)
        {
            List<ILabelElement> list = Converter.toLabelElements(item.elements, bin, part, assetPath);
            return new(item.padding, item.minScale ?? 0.5, item.maxScale, list);
        }
    }
}
