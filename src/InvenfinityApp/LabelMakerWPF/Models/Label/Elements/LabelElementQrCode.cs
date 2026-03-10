using LabelMakerWPF.Services;
using QRCoder;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabelMaker.Models.Label.Elements
{
    internal class LabelElementQrCode : ILabelElement
    {
        public string value;
        public int? MinWidthMm { get; private set; }
        public double? Padding { get; private set; }
        public double minScale { get; private set; }
        public double maxScale { get; private set; }
        public static string Name => "qrcode";
        public LabelElementQrCode(string  value, int? widthMm, double? padding, double minScale, double maxScale)
        {
            this.value = value;
            this.MinWidthMm = widthMm;
            this.Padding = padding;
            this.minScale = minScale;
            this.maxScale = maxScale;
        }

        public double GetWidth(double labelHeight, double scale)
        {
            // Breite = Höhe beim QR-Code, aber skaliert!
            return labelHeight * scale;
        }

        public void Render(DrawingGroup group, double x, double labelHeight, double scale)
        {
            var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
            var svgQr = new SvgQRCode(data);
            string svg = svgQr.GetGraphic(1, "#000000", "#FFFFFF", false);

            var reader = new FileSvgReader(new WpfDrawingSettings());
            DrawingGroup drawing;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(svg)))
            {
                drawing = reader.Read(stream);
            }

            if (drawing == null) return;

            // Zielgröße berechnen
            double size = labelHeight * scale;
            // Vertikal zentrieren
            double yOffset = (labelHeight - size) / 2;

            SvgHelper.DrawSvg(
                group,
                drawing,
                x,
                yOffset,
                size,   // Zielbreite
                size);  // Zielhöhe
        }
    }
}
