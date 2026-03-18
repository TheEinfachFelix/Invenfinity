using LabelMaker.Templates.Json;
using LabelMakerWPF.Models.Label.Elements;
using LabelMakerWPF.Services;
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
    internal class LabelElementImage : LabelElementBase, ILabelElement
    {
        private readonly Drawing _svgDrawing;
        private readonly double _aspectRatio;
        public static string Name => "image";
        public LabelElementImage(Drawing svgDrawing, double padding, double minScale, double maxScale, HorisontalAlignCases hori, VerticalAlignCases vert, OrientationCases orient)
            : base( padding, minScale, maxScale, hori, vert, orient)
        {
            _svgDrawing = svgDrawing ?? throw new ArgumentNullException(nameof(svgDrawing));

            // Aspect Ratio einmal berechnen (Breite / Höhe)
            var bounds = _svgDrawing.Bounds;
            _aspectRatio = bounds.Height != 0 ? bounds.Width / bounds.Height : 1.0;
        }

        public override DrawingGroup Render(double labelHeightUnits, double labelLengthUnits)
        {
            return LayoutHelper.CreateDrawGroup(_svgDrawing, this, labelLengthUnits, labelHeightUnits);
        }

        public override DrawingGroup RenderStandardSize(double labelHeightUnits)
        {
            return Render(labelHeightUnits, labelHeightUnits * _aspectRatio);
        }
        public static LabelElementImage GenerateElement(LayoutItem item, Drawing svgDrawing)
        {
            HorisontalAlignCases hori = Enum.Parse<HorisontalAlignCases>(item.horisontalAlign, true);
            VerticalAlignCases vert = Enum.Parse<VerticalAlignCases>(item.verticalAlign, true);
            OrientationCases orient = Enum.Parse<OrientationCases>(item.orientation, true);
            return new(svgDrawing, item.padding, item.minScale, item.maxScale, hori, vert, orient);
        }
    }
}
