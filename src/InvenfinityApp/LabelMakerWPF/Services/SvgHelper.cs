using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LabelMakerWPF.Services
{
    internal class SvgHelper
    {
        public static void DrawSvg(
    DrawingGroup target,
    DrawingGroup svg,
    double x,
    double y,
    double width,
    double height)
        {
            if (svg == null) return;

            Rect bounds = svg.Bounds;

            if (bounds.Width == 0 || bounds.Height == 0)
                return;

            double scale = Math.Min(width / bounds.Width, height / bounds.Height);

            double scaledWidth = bounds.Width * scale;
            double scaledHeight = bounds.Height * scale;

            double offsetX = x + (width - scaledWidth) / 2 - bounds.X * scale;
            double offsetY = y + (height - scaledHeight) / 2 - bounds.Y * scale;

            var tg = new TransformGroup();
            tg.Children.Add(new ScaleTransform(scale, scale));
            tg.Children.Add(new TranslateTransform(offsetX, offsetY));

            svg.Transform = tg;

            target.Children.Add(svg);
        }
    }
}
