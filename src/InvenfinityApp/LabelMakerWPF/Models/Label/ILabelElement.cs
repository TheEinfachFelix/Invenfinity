using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMaker.Models.Label
{
    internal interface ILabelElement
    {
        public int? MinWidthMm { get; }
        public double? Padding { get; }
        void Render(DrawingGroup group, double x, double labelHeight, double scale);
        double GetWidth(double labelHeight, double scale);
        public static string Name { get; }

        public double minScale { get; }
        public double maxScale { get; }
    }
}
