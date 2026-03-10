using LabelMaker.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMaker.Models.Label
{
    internal class LabelRoot
    {
        public double LabelLength { get; set; }
        public List<ILabelElement> Elements { get; set; } = new List<ILabelElement>();
        public LabelRoot(double labelLength) 
        {
            LabelLength = labelLength;
        }

        public DrawingGroup BuildVector(double labelHeight)
        {
            var group = new DrawingGroup();

            double xOffset = 0;

            foreach (var element in Elements)
            {
                if (element.MinWidthMm.HasValue &&
                    LabelLength < element.MinWidthMm)
                    continue;

                element.Render(group, xOffset, labelHeight);

                xOffset += element.GetWidth(labelHeight);

                if (element.Padding.HasValue)
                    xOffset += Converter.mmtoUnits(element.Padding.Value);
            }

            return group;
        }
    }
}
