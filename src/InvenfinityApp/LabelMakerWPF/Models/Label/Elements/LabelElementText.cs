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
        public double? Padding { get; private set; }
        public double minScale { get; private set; }
        public double maxScale { get; private set; }
        public string? splitChar { get; private set; }
        public static string Name => "text";
        public LabelElementText(string text, int? widthMm, double? padding, double minScale, double maxScale, string? splitChar) 
        { 
            this.Text = text;
            this.MinWidthMm = widthMm;
            this.Padding = padding;
            this.minScale = minScale;
            this.maxScale = maxScale;
            this.splitChar = splitChar;
        }
        private FormattedText GetText(double labelHeight, double scale)
        {
            if (scale < minScale || scale > maxScale) throw new ArgumentOutOfRangeException(nameof(scale));
            double fontSize = labelHeight * scale;
            var thisText = Text;
            if (scale <= 0.45 &&  splitChar != null)
            {
                int middleIndex = -1;
                int count = 0;

                // Zähle, wie viele splitChar vorkommen
                for (int i = 0; i < thisText.Length; i++)
                {
                    if (thisText[i].ToString() == splitChar)
                        count++;
                }

                if (count > 0)
                {
                    int targetOccurrence = (count + 1) / 2; // mittlere Occurrence
                    int occurrence = 0;

                    // finde die Position des mittleren splitChar
                    for (int i = 0; i < thisText.Length; i++)
                    {
                        if (thisText[i].ToString() == splitChar)
                        {
                            occurrence++;
                            if (occurrence == targetOccurrence)
                            {
                                middleIndex = i;
                                break;
                            }
                        }
                    }

                    if (middleIndex >= 0)
                    {
                        // mittleren splitChar durch \n ersetzen
                        thisText = thisText.Substring(0, middleIndex) + "\n" + splitChar + thisText.Substring(middleIndex + 1);
                    }
                }
            }


            return new(
                thisText,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                Brushes.Black,
                96);
        }
        public double GetWidth(double labelHeight, double scale)
        {


            return GetText(labelHeight,scale).Width;
        }

        public void Render(DrawingGroup group, double x, double labelHeight, double scale)
        {
            var formatted = GetText(labelHeight,scale);

            var drawing = new GeometryDrawing(
                Brushes.Black,
                null,
                formatted.BuildGeometry(
                    new Point(x, (labelHeight - formatted.Height) / 2)));

            group.Children.Add(drawing);
        }
    }
}
