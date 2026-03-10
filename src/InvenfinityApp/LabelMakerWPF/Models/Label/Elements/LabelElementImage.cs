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
    internal class LabelElementImage : ILabelElement
    {
        private string path;
        private string type;
        private string name;
        public int? MinWidthMm { get; private set; }
        public double? Padding { get; private set; }
        public static string Name => "image";
        public string Path => path + type + "/" + name + ".svg";
        public double minScale { get; private set; }
        public double maxScale { get; private set; }
        public LabelElementImage(string path,string type, string name, int? widthMm, double? padding, double minScale, double maxScale)
        {
            this.path = path;
            this.type = type;
            this.name = name;
            this.MinWidthMm = widthMm;
            this.Padding = padding;
            this.minScale = minScale;
            this.maxScale = maxScale;

        }



        public void Render(DrawingGroup group, double x, double labelHeight, double scale)
        {
            double targetHeight = labelHeight * scale;
            // Vertikale Zentrierung:
            double yOffset = (labelHeight - targetHeight) / 2;

            if (Path.EndsWith(".svg"))
                RenderSvg(group, x, yOffset, targetHeight);
            else
                RenderPng(group, x, yOffset, targetHeight);
        }

        // Hilfsmethode leicht angepasst (y und targetHeight als Parameter)
        void RenderSvg(DrawingGroup group, double x, double y, double targetHeight)
        {
            var reader = new FileSvgReader(new WpfDrawingSettings());
            var drawing = reader.Read(Path);
            if (drawing == null) return;

            var bounds = drawing.Bounds;
            double s = targetHeight / bounds.Height;

            var tg = new TransformGroup();
            tg.Children.Add(new ScaleTransform(s, s));
            tg.Children.Add(new TranslateTransform(x - bounds.X * s, y - bounds.Y * s));

            drawing.Transform = tg;
            group.Children.Add(drawing);
        }

        void RenderPng(DrawingGroup group, double x, double y, double targetHeight)
        {
            var bmp = new BitmapImage(new Uri(Path, UriKind.Absolute));
            double s = targetHeight / bmp.PixelHeight;
            double width = bmp.PixelWidth * s;

            var rect = new Rect(x, y, width, targetHeight);
            group.Children.Add(new ImageDrawing(bmp, rect));
        }
        public double GetWidth(double labelHeight, double scale)
        {
            double scaledHeight = labelHeight * scale;
            if (Path.EndsWith(".svg"))
                return GetSvgWidth(scaledHeight);
            else
                return GetPngWidth(scaledHeight);
        }
        double GetSvgWidth(double labelHeight)
        {
            var reader = new FileSvgReader(new WpfDrawingSettings());
            var drawing = reader.Read(Path);

            if (drawing == null)
                return labelHeight;

            var bounds = drawing.Bounds;

            if (bounds.Height == 0)
                return labelHeight;

            double scale = labelHeight / bounds.Height;

            return bounds.Width * scale;
        }
        double GetPngWidth(double labelHeight)
        {
            var bmp = new BitmapImage(new Uri(Path, UriKind.Absolute));

            if (bmp.PixelHeight == 0)
                return labelHeight;

            double scale = labelHeight / bmp.PixelHeight;

            return bmp.PixelWidth * scale;
        }

    }
}
