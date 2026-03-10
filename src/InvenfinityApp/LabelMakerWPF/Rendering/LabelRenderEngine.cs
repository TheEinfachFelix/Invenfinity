using LabelMaker.Models.Label;
using LabelMaker.Models.Label.Elements;
using LabelMaker.Services;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace LabelMaker.Rendering
{
    internal class LabelRenderEngine
    {
        public void Print(LabelRoot label)
        {
            double labelHeight = Converter.mmtoUnits(12);
            double labelWidth = Converter.mmtoUnits(label.LabelLength);

            var vector = label.BuildVector(labelHeight);

            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                dc.DrawDrawing(vector);
            }

            var pd = new PrintDialog();
            pd.PrintVisual(visual, "Label Druck");
        }

        public RenderTargetBitmap ToBitmap(DrawingGroup vector, double width, double height)
        {
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
                dc.DrawDrawing(vector);

            var bmp = new RenderTargetBitmap(
                (int)width,
                (int)height,
                96,
                96,
                PixelFormats.Pbgra32);

            bmp.Render(visual);

            return bmp;
        }
    }
}