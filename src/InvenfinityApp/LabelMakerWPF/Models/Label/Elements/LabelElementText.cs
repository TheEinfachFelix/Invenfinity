using LabelMakerWPF.Models.Label.Elements;
using LabelMakerWPF.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LabelMaker.Models.Label.Elements
{
    internal class LabelElementText : LabelElementBase
    {
        private Typeface typeface = new (new FontFamily("Segoe UI"),FontStyles.Normal,FontWeights.Normal,FontStretches.Normal);
        private const double AUTO_SPLIT_THRESHOLD = 0.45;
        public string Text { get; private set; }
        public string? SplitChar { get; private set; }
        public static string Name => "text";
        public LabelElementText(string text, int? widthMm, double? padding, double minScale, double maxScale, string? splitChar)
            : base(widthMm, padding, minScale, maxScale)
        { 
            this.Text = text;
            this.SplitChar = splitChar;
        }
        private FormattedText GetText(double labelHeight, double scale)
        {
            if (scale < MinScale || scale > MaxScale) throw new ArgumentOutOfRangeException(nameof(scale));

            double fontSize = labelHeight * scale;
            var thisText = Text;

            // 1. Bestehende Logik für splitChar (Umbruch)
            if (scale <= AUTO_SPLIT_THRESHOLD && SplitChar != null)
            {
                thisText = TextParser.SplitTextCenter(thisText, SplitChar);
            }

            // 2. Logik für Fettdruck (*text*)
            var bold = TextParser.BoldFinder(thisText);
            var boldRanges = bold.Item1;
            var finalBuilder = bold.Item2;


            string displayText = finalBuilder.ToString();

            // 3. FormattedText erstellen
            var formatted = new FormattedText(
                displayText,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                Brushes.Black,
                96);

            // 4. Die gespeicherten Bereiche fett formatieren
            foreach (var range in boldRanges)
            {
                formatted.SetFontSize(fontSize + 3, range.start, range.length);
                formatted.SetFontWeight(FontWeights.Bold, range.start, range.length);
            }

            return formatted;
        }
        public override double GetWidth(double labelHeight, double scale)
        {
            return GetText(labelHeight,scale).Width;
        }

        public override void Render(DrawingGroup group, double x, double labelHeight, double scale)
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
