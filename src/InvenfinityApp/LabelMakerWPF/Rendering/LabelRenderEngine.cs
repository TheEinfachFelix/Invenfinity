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
        public RenderTargetBitmap Preview(LabelRoot label)
        {
            double labelHeight = Converter.mmtoUnits(12);
            double labelWidth = Converter.mmtoUnits(label.LabelLength);
            var bmp = new RenderTargetBitmap((int)labelWidth, (int)labelHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(Draw(label));
            return bmp; // ImageControl im UI
        }

        public void Print(LabelRoot label)
        {
            var pd = new PrintDialog();
            pd.PrintVisual(Draw(label), "Label Druck");
        }
            
        private DrawingVisual Draw(LabelRoot label)
        {
            double labelHeight = Converter.mmtoUnits(12);
            double labelWidth = Converter.mmtoUnits(label.LabelLength);
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(Brushes.Wheat, null, new Rect(0, 0, labelWidth, labelHeight));
                double xOffset = 0;

                foreach (var element in label.Elements)
                {
                    double elementWidth = element.WidthMm.HasValue ? Converter.mmtoUnits(element.WidthMm.Value) : 12; // Problem hier
                    double elementPadding = element.Padding.HasValue ? Converter.mmtoUnits(element.Padding.Value) : 2;

                    switch (element)
                    {
                        case LabelElementText text:
                            DrawText(dc, text, xOffset + elementPadding, labelHeight);
                            break;
                        case LabelElementImage image:
                            DrawImage(dc, image, xOffset + elementPadding, labelHeight);
                            break;
                        case LabelElementQrCode qr:
                            DrawQrCode(dc, qr, xOffset + elementPadding, labelHeight);
                            break;
                    }

                    xOffset += elementWidth + elementPadding;
                }
            }
            return visual;
        }

        void DrawText(DrawingContext dc, LabelElementText text, double x, double labelHeight)
        {
            var fontSize = labelHeight * 0.6; // 60 % der Höhe
            var typeface = new Typeface(new FontFamily("Roboto Condensed"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            var formattedText = new FormattedText(
                text.Text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                Brushes.Black,
                96); // dpi

            dc.DrawText(formattedText, new Point(x, (labelHeight - formattedText.Height) / 2));
        }
        
        void DrawImage(DrawingContext dc, LabelElementImage image, double x, double labelHeight)
        {
            if (image.Path.EndsWith(".svg"))
                DrawImageSVG(dc, image, x, labelHeight);
            else
                DrawImagePNG(dc, image, x, labelHeight);

        }
        DrawingGroup LoadSvgDrawing(string filePath)
        {
            var settings = new WpfDrawingSettings { IncludeRuntime = true };
            var reader = new FileSvgReader(settings);
            var drawing = reader.Read(filePath); // returns DrawingGroup
            return drawing; // kann direkt gezeichnet werden
        }

        void DrawImageSVG(DrawingContext dc, LabelElementImage image, double x, double labelHeight)
        {
            var drawing = LoadSvgDrawing(image.Path);
            if (drawing == null) return;

            // Skalierung
            Rect bounds = drawing.Bounds;
            if (bounds.Width == 0 || bounds.Height == 0) return;
            double scale = labelHeight / bounds.Height;

            var tg = new TransformGroup();
            tg.Children.Add(new ScaleTransform(scale, scale));
            tg.Children.Add(new TranslateTransform(x, 0));
            drawing.Transform = tg;

            dc.DrawDrawing(drawing);
        }
        void DrawImagePNG(DrawingContext dc, LabelElementImage image, double x, double labelHeight)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(image.Path, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            if (bitmap.PixelWidth == 0 || bitmap.PixelHeight == 0) throw new Exception("sadfsadfasdf"); // Sicherheitscheck

            double scale = labelHeight / bitmap.PixelHeight;
            double imgWidth = bitmap.PixelWidth * scale;
            double imgHeight = labelHeight;

            dc.DrawImage(bitmap, new Rect(x, 0, imgWidth, imgHeight));
        }

        void DrawQrCode(DrawingContext dc, LabelElementQrCode qr, double x, double labelHeight)
        {
            using var generator = new QRCoder.QRCodeGenerator();
            var data = generator.CreateQrCode(qr.value, QRCoder.QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCoder.QRCode(data);
            using var bitmap = qrCode.GetGraphic(20); // Pixelgröße
            var bmp = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight((int)labelHeight, (int)labelHeight)
            );

            dc.DrawImage(bmp, new Rect(x, 0, labelHeight, labelHeight));
        }
    }
}
