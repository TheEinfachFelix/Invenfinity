using LabelMaker.Models.Label;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMakerWPF.Models.Label.Elements
{
    internal abstract class LabelElementBase : ILabelElement
    {
        public int? MinWidthMm { get; }
        public double? Padding { get; }
        public double MinScale { get; }
        public double MaxScale { get; }

        protected LabelElementBase(int? widthMm, double? padding, double minScale, double maxScale)
        {
            MinWidthMm = widthMm;
            Padding = padding;
            MinScale = minScale;
            MaxScale = maxScale;
        }

        public abstract double GetWidth(double labelHeight, double scale);
        public abstract void Render(DrawingGroup group, double x, double labelHeight, double scale);

        // Hilfsmethode für alle Elemente
        protected double CalculateYOffset(double labelHeight, double targetHeight)
            => (labelHeight - targetHeight) / 2;
    }
}
