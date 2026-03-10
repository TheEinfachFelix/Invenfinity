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
        public int? Padding { get; private set; }
        public static string Name => "image";
        public string Path => path + type + "/" + name + ".svg";
        public LabelElementImage(string path,string type, string name, int? widthMm, int? padding)
        {
            this.path = path;
            this.type = type;
            this.name = name;
            this.MinWidthMm = widthMm;
            this.Padding = padding;

        }



        public void Render(DrawingGroup group, double x, double labelHeight)
        {
            if (Path.EndsWith(".svg"))
                RenderSvg(group, x, labelHeight);
            else
                RenderPng(group, x, labelHeight);
        }

        void RenderSvg(DrawingGroup group, double x, double labelHeight)
        {
            var reader = new FileSvgReader(new WpfDrawingSettings());
            var drawing = reader.Read(Path);

            if (drawing == null)
                return;

            var bounds = drawing.Bounds;
            double scale = labelHeight / bounds.Height;

            var tg = new TransformGroup();
            tg.Children.Add(new ScaleTransform(scale, scale));

            tg.Children.Add(
                new TranslateTransform(
                    x - bounds.X * scale,
                    -bounds.Y * scale));

            drawing.Transform = tg;

            group.Children.Add(drawing);
        }
        public double GetWidth(double labelHeight)
        {
            if (Path.EndsWith(".svg"))
                return GetSvgWidth(labelHeight);
            else
                return GetPngWidth(labelHeight);
        }

        void RenderPng(DrawingGroup group, double x, double labelHeight)
        {
            var bmp = new BitmapImage(new Uri(Path, UriKind.Absolute));

            double scale = labelHeight / bmp.PixelHeight;
            double width = bmp.PixelWidth * scale;

            var rect = new Rect(x, 0, width, labelHeight);

            group.Children.Add(new ImageDrawing(bmp, rect));
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
