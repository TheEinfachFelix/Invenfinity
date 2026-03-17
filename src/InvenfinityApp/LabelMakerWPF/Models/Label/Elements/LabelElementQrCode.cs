using LabelMaker.Templates.Json;
using LabelMakerWPF.Models.Label.Elements;
using LabelMakerWPF.Services;
using QRCoder;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabelMaker.Models.Label.Elements
{
    internal class LabelElementQrCode : LabelElementBase, ILabelElement
    {
        private Drawing _drawing;
        public string value;
        public static string Name => "qrcode";
        public LabelElementQrCode(string  value, double padding, double minScale, double maxScale, HorisontalAlignCases hori, VerticalAlignCases vert, OrientationCases orient)
            : base( padding, minScale, maxScale, hori, vert, orient)
        {
            this.value = value;

            _drawing = SvgHelper.GenerateQrCode(value);
        }

        public override DrawingGroup Render(double labelHeightUnits, double labelLengthUnits)
        {
            return SvgHelper.DrawSvg(
                _drawing,
                this,
                labelLengthUnits,
                labelHeightUnits);
        }

        public override DrawingGroup RenderStandardSize(double labelHeightUnits)
        {
            return Render(labelHeightUnits, labelHeightUnits);
        }

        public static LabelElementQrCode GenerateElement(LayoutItem item, string resolvedValue)
        {
            HorisontalAlignCases hori = Enum.Parse<HorisontalAlignCases>(item.horisontalAlign, true);
            VerticalAlignCases vert = Enum.Parse<VerticalAlignCases>(item.verticalAlign, true);
            OrientationCases orient = Enum.Parse<OrientationCases>(item.orientation, true);
            return new(resolvedValue, item.padding, item.minScale, item.maxScale, hori, vert, orient);
        }
    }
}
