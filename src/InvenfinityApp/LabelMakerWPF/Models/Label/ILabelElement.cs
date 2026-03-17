using LabelMaker.Templates.Json;
using LabelMakerWPF.Models.Label.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMaker.Models.Label
{
    internal interface ILabelElement
    {
        public double Padding { get; }
        public double PaddingUnits { get; }
        DrawingGroup Render(double labelHeightUnits, double labelLengthUnits);
        DrawingGroup RenderStandardSize(double labelHeightUnits);
        public static string Name { get; } = "NotSet";
        public double MinScale { get; }
        public double MaxScale { get; }
        public HorisontalAlignCases HorisontalAlign { get; }
        public VerticalAlignCases VerticalAlign { get; }
        public OrientationCases Orientation { get; }

        internal double getXOffest(double labelLengthUnits, double contentLengthUnits);
        internal double getYOffest(double labelHeightUnits, double contentHeightUnits);
    }
}
