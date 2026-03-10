using LabelMakerWPF.Models.Label.Elements;
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
    internal class LabelElementQrCode : LabelElementBase
    {
        private Drawing _drawing;
        public string value;
        public static string Name => "qrcode";
        public LabelElementQrCode(string  value, int? widthMm, double? padding, double minScale, double maxScale)
            : base(widthMm, padding, minScale, maxScale)
        {
            this.value = value;

            _drawing = SvgHelper.GenerateQrCode(value);
        }

        public override double GetWidth(double labelHeight, double scale)
        {
            // Breite = Höhe beim QR-Code, aber skaliert!
            return labelHeight * scale;
        }

        public override void Render(DrawingGroup group, double x, double labelHeight, double scale)
        {


            if (_drawing == null) return;

            // Zielgröße berechnen
            double size = labelHeight * scale;
            // Vertikal zentrieren
            double yOffset = (labelHeight - size) / 2;

            SvgHelper.DrawSvg(
                group,
                _drawing,
                x,
                yOffset,
                size,   // Zielbreite
                size);  // Zielhöhe
        }
    }
}
