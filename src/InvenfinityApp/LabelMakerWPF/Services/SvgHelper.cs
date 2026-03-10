using QRCoder;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LabelMakerWPF.Services
{
    internal static class SvgHelper
    {
        public static void DrawSvg(
            DrawingGroup target,
            Drawing svgSource, // Das Original aus dem Cache
            double x,
            double y,
            double targetWidth,
            double targetHeight)
        {
            if (svgSource == null) return;

            Rect bounds = svgSource.Bounds;
            if (bounds.Width == 0 || bounds.Height == 0) return;

            double scale = Math.Min(targetWidth / bounds.Width, targetHeight / bounds.Height);

            // Wir erstellen einen neuen Container für den Render-Vorgang
            var wrapper = new DrawingGroup();

            var tg = new TransformGroup();
            // 1. Skalieren
            tg.Children.Add(new ScaleTransform(scale, scale));
            // 2. Positionieren (Offset-Korrektur für Bounds.X/Y und Zentrierung)
            double offsetX = x - (bounds.X * scale) + (targetWidth - bounds.Width * scale) / 2;
            double offsetY = y - (bounds.Y * scale) + (targetHeight - bounds.Height * scale) / 2;
            tg.Children.Add(new TranslateTransform(offsetX, offsetY));

            wrapper.Transform = tg;
            wrapper.Children.Add(svgSource); // Das Original bleibt unverändert

            target.Children.Add(wrapper);
        }
        public static Drawing GenerateQrCode(string value)
        {
            var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
            var svgQr = new SvgQRCode(data);
            string svg = svgQr.GetGraphic(1, "#000000", "#FFFFFF", false);

            var reader = new FileSvgReader(new WpfDrawingSettings());

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(svg)))
            {
                return reader.Read(stream);
            }
        }
    }
}
