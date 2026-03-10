using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LabelMaker.Models.Label.Elements
{
    internal class LabelElementText : ILabelElement
    {
        private Typeface typeface = new (
                new FontFamily("Segoe UI"),
                FontStyles.Normal,
                FontWeights.Normal,
                FontStretches.Normal);
        public string Text { get; private set; }
        public int? MinWidthMm { get; private set; }
        public int? Padding { get; private set; }
        public static string Name => "text";
        public LabelElementText(string text, int? widthMm, int? padding) 
        { 
            this.Text = text;
            this.MinWidthMm = widthMm;
            this.Padding = padding;

        }

        public double GetWidth(double labelHeight)
        {
            double fontSize = labelHeight * 0.6;

            var formatted = new FormattedText(
                Text,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                Brushes.Black,
                96);

            return formatted.Width;
        }

        public void Render(DrawingGroup group, double x, double labelHeight)
        {
            double fontSize = labelHeight * 0.6;



            var formatted = new FormattedText(
                Text,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                Brushes.Black,
                96);

            var drawing = new GeometryDrawing(
                Brushes.Black,
                null,
                formatted.BuildGeometry(
                    new Point(x, (labelHeight - formatted.Height) / 2)));

            group.Children.Add(drawing);
        }
        }
}
