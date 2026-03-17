using LabelMaker.Services;
using LabelMaker.Templates.Json;
using LabelMakerWPF.Models.Label.Elements;
using LabelMakerWPF.Services;
using SharpVectors.Dom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LabelMaker.Models.Label.Elements
{
    internal class LabelElementText : LabelElementBase, ILabelElement
    {
        private Typeface typeface = new (new FontFamily("Segoe UI"),FontStyles.Normal,FontWeights.Normal,FontStretches.Normal);
        public string Text { get; private set; }
        public static string Name => "text";
        public LabelElementText(string text, double? padding, double minScale, double maxScale, HorisontalAlignCases hori, VerticalAlignCases vert, OrientationCases orient)
            : base(padding, minScale, maxScale, hori, vert, orient)
        { 
            this.Text = text;
        }
        private FormattedText GetText(double labelHeight, double scale)
        {
            if (scale > MaxScale || scale < MinScale) throw new ArgumentOutOfRangeException(nameof(scale));

            double fontSize = labelHeight * scale;
            var thisText = Text;

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

        public override DrawingGroup Render(double labelHeightUnits, double labelLengthUnits)
        {
            double standardLength = GetStandardLength(labelHeightUnits);
            double scale = standardLength / labelLengthUnits;
            if (scale > MaxScale) scale = MaxScale;
            if (scale < MinScale) throw new ArgumentOutOfRangeException(nameof(scale));

            var text = GetText(labelHeightUnits, scale);
            double height = text.Height;
            double length = text.Width;

            // TODO Orientation

            

            GeometryDrawing drawing = new(
            Brushes.Black,
            null,
            text.BuildGeometry(
                new Point(getXOffest(labelLengthUnits,length), getYOffest(labelHeightUnits, height))));

            // TODO setLength
            return SvgHelper.DrawSvg(drawing, this, labelLengthUnits, labelHeightUnits);
        }

        public override DrawingGroup RenderStandardSize(double labelHeightUnits)
        {
            return Render(labelHeightUnits, GetStandardLength(labelHeightUnits));
        }

        private double GetStandardLength(double labelHeightUnits)
        {
            var formatted = GetText(labelHeightUnits, 1);

            var drawing = new GeometryDrawing(
                Brushes.Black,
                null,
                formatted.BuildGeometry(
                    new Point(0, 0)));
            return drawing.Bounds.Width + PaddingUnits;
        }

        public static LabelElementText GenerateElement(LayoutItem item, string resolvedValue)
        {
            HorisontalAlignCases hori = Enum.Parse<HorisontalAlignCases>(item.horisontalAlign, true);
            VerticalAlignCases vert = Enum.Parse<VerticalAlignCases>(item.verticalAlign, true);
            OrientationCases orient = Enum.Parse<OrientationCases>(item.orientation, true);
            return new LabelElementText(resolvedValue, item.padding, item.minScale, item.maxScale, hori, vert, orient);
        }
    }
}
