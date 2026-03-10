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
        public int? Padding { get; private set; }
        public static string Name => "qrcode";
        public LabelElementQrCode(string  value, int? widthMm, int? padding)
        {
            this.value = value;
            this.MinWidthMm = widthMm;
            this.Padding = padding;

        }

        public double GetWidth(double labelHeight)
        {
            return labelHeight; // QR ist quadratisch
        }
        public void Render(DrawingGroup group, double x, double labelHeight)
        {
            var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);

            var svgQr = new SvgQRCode(data);
            string svg = svgQr.GetGraphic(1);

            var reader = new FileSvgReader(new WpfDrawingSettings());

            DrawingGroup drawing;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(svg)))
            {
                drawing = reader.Read(stream);
            }

            if (drawing == null)
                return;

            SvgHelper.DrawSvg(
                group,
                drawing,
                x,
                0,
                labelHeight,
                labelHeight);
        }
    }
}
