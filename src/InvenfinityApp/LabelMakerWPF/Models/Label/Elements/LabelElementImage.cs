using LabelMakerWPF.Models.Label.Elements;
using LabelMakerWPF.Services;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabelMaker.Models.Label.Elements
{
    internal class LabelElementImage : LabelElementBase
    {
        private readonly Drawing _svgDrawing;
        private readonly double _aspectRatio;
        public static string Name => "image";
        public LabelElementImage(Drawing svgDrawing, int? widthMm, double? padding, double minScale, double maxScale)
            : base(widthMm, padding, minScale, maxScale)
        {
            _svgDrawing = svgDrawing ?? throw new ArgumentNullException(nameof(svgDrawing));

            // Aspect Ratio einmal berechnen (Breite / Höhe)
            var bounds = _svgDrawing.Bounds;
            _aspectRatio = bounds.Height != 0 ? bounds.Width / bounds.Height : 1.0;
        }

        public override double GetWidth(double labelHeight, double scale)
        {
            return (labelHeight * scale) * _aspectRatio;
        }

        public override void Render(DrawingGroup group, double x, double labelHeight, double scale)
        {
            double targetHeight = labelHeight * scale;
            double targetWidth = GetWidth(labelHeight, scale);
            double y = CalculateYOffset(labelHeight, targetHeight);

            SvgHelper.DrawSvg(group, _svgDrawing, x, y, targetWidth, targetHeight);
        }
    }
}
