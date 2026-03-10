using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMaker.Models.Label
{
    internal interface ILabelElement
    {
        public int? MinWidthMm { get; }
        public int? Padding { get; }
        void Render(DrawingGroup group, double x, double labelHeight);
        double GetWidth(double labelHeight);
        public static string Name { get; }
    }
}
