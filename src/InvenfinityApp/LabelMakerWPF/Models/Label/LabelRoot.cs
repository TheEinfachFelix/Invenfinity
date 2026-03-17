using LabelMaker.Services;
using LabelMakerWPF.Services;
using LabelMakerWPF.Templates.Printer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMaker.Models.Label
{
    internal class LabelRoot
    {
        public double LabelLength { get; set; }
        public List<ILabelElement> Elements { get; set; } = [];
        public LabelRoot(double labelLength) 
        {
            LabelLength = labelLength;
        }

        public DrawingGroup BuildVector(double labelHeight)
        {
            var group = new DrawingGroup();
            var elements = Elements;

            if (!elements.Any()) return group;

            // 1. Berechne die individuellen Skalierungswerte für die feste Länge

            double xOffset = 0;

            var list = new List<DrawingGroup>();
            foreach (var item in elements)
            {
                list.Add(item.RenderStandardSize(labelHeight));
            }
            return SvgHelper.concadGroups(list);

            //for (int i = 0; i < elements.Count; i++)
            //{
            //    var element = elements[i];
            //    double currentScale = 1;

            //    // Rendern mit der berechneten Skalierung
            //    element.Render(group, xOffset, labelHeight, currentScale);

            //    // Offset erhöhen
            //    xOffset += element.GetWidth(labelHeight, currentScale);

            //    if (element.Padding.HasValue)
            //        xOffset += Converter.mmtoUnits(element.Padding.Value);
            //}

            //return group;
        }
        
    }
}
