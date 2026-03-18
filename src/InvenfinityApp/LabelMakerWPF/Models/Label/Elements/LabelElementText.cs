using LabelMaker.Services;
using LabelMaker.Templates.Json;
using LabelMakerWPF.Models.Label.Elements;
using LabelMakerWPF.Services;
using SharpVectors.Dom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace LabelMaker.Models.Label.Elements
{
    internal class LabelElementText : LabelElementBase, ILabelElement
    {
        private Typeface typeface = new (new FontFamily("Segoe UI"),FontStyles.Normal,FontWeights.Normal,FontStretches.Normal);
        public string Text { get; private set; }
        public static string Name => "text";
        public LabelElementText(string text, double padding, double minScale, double maxScale, HorisontalAlignCases hori, VerticalAlignCases vert, OrientationCases orient)
            : base(padding, minScale, maxScale, hori, vert, orient)
        { 
            this.Text = text;
        }
        private GeometryDrawing GetText(double labelHeight)
        {
            double fontSize = labelHeight;
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

            GeometryDrawing drawing = new(
                Brushes.Black,
                null,
                formatted.BuildGeometry(
                    new Point(0, 0)));

            return drawing;
        }

        public override DrawingGroup Render(double labelHeightUnits, double labelLengthUnits)
        {
            var text = GetText(labelHeightUnits);
            return LayoutHelper.CreateDrawGroup(text, this, labelLengthUnits, labelHeightUnits);
        }

        public override DrawingGroup RenderStandardSize(double labelHeightUnits)
        {
            return Render(labelHeightUnits, GetStandardLength(labelHeightUnits));
        }

        private double GetStandardLength(double labelHeightUnits)
        {
            var formatted = GetText(labelHeightUnits);
            var bounds = formatted.Bounds;
            switch (Orientation)
            {
                case OrientationCases.Vertical:
                    return bounds.Height + PaddingUnits;
                case OrientationCases.Wide:
                    // Wenn es höher als breit ist, rotiere es ins Querformat
                    if (bounds.Width < bounds.Height) return bounds.Height + PaddingUnits;
                    break;
                case OrientationCases.Narrow:
                    // Wenn es breiter als hoch ist, rotiere es ins Hochformat
                    if (bounds.Width > bounds.Height) return bounds.Height + PaddingUnits;
                    break;
            }

            return bounds.Width + PaddingUnits;
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
