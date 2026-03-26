using LabelMaker.Models.Label;
using LabelMaker.Templates.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMakerWPF.Models.Label.Elements
{
    internal class LabelElementPart : LabelElementBase, ILabelElement
    {
        public static string Name { get; } = "partElement";

        public LabelElementPart(double padding, double minScale, double maxScale, HorisontalAlignCases hori, VerticalAlignCases vert, OrientationCases orient) : base(padding, minScale, maxScale, hori, vert, orient)
        {
        }

        public override DrawingGroup Render(double labelHeightUnits, double labelLengthUnits)
        {
            throw new NotImplementedException();
        }

        public override DrawingGroup RenderStandardSize(double labelHeightUnits)
        {
            throw new NotImplementedException();
        }

        public static LabelElementPart GenerateElement(LayoutItem item)
        {
            HorisontalAlignCases hori = Enum.Parse<HorisontalAlignCases>(item.horisontalAlign, true);
            VerticalAlignCases vert = Enum.Parse<VerticalAlignCases>(item.verticalAlign, true);
            OrientationCases orient = Enum.Parse<OrientationCases>(item.orientation, true);
            return new(item.padding, item.minScale, item.maxScale, hori, vert, orient);
        }
    }
}
