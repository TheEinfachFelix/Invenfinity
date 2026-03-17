using LabelMaker.Models.Label;
using LabelMaker.Services;
using LabelMaker.Templates.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Windows.Media;

namespace LabelMakerWPF.Models.Label.Elements
{
    internal abstract class LabelElementBase : ILabelElement
    {
        public int? MinWidthMm { get; }
        public double? Padding { get; }
        public double PaddingUnits { get { return Converter.mmtoUnits(Padding ?? 0); } }
        public double MinScale { get; }
        public double MaxScale { get; }
        public HorisontalAlignCases HorisontalAlign { get; }
        public VerticalAlignCases VerticalAlign { get; }
        public OrientationCases Orientation { get; }

        protected LabelElementBase(double? padding, double minScale, double maxScale, HorisontalAlignCases hori, VerticalAlignCases vert, OrientationCases orient)
        {
            Padding = padding;
            MinScale = minScale;
            MaxScale = maxScale;
            HorisontalAlign = hori;
            VerticalAlign = vert;
            Orientation = orient;
        }
        public double getXOffest (double labelLengthUnits, double contentLengthUnits)
        {
            switch (HorisontalAlign)
            {
                case HorisontalAlignCases.Left:
                    return 0;
                case HorisontalAlignCases.Right:
                    return labelLengthUnits - contentLengthUnits;
                case HorisontalAlignCases.Center:
                    return (labelLengthUnits - contentLengthUnits) / 2;
                default:
                    throw new Exception("HorisontalAlignCase not found");
            }
        }
        public double getYOffest (double  labelHeightUnits, double contentHeightUnits)
        {
            switch (VerticalAlign)
            {
                case VerticalAlignCases.Top:
                    return 0;
                case VerticalAlignCases.Bottom:
                    return labelHeightUnits - contentHeightUnits;
                case VerticalAlignCases.Center:
                    return (labelHeightUnits - contentHeightUnits) / 2;
                default:
                    throw new Exception("VerticalAlignCase not found");
            }
        }

        public abstract DrawingGroup Render(double labelHeightUnits, double labelLengthUnits);

        public abstract DrawingGroup RenderStandardSize(double labelHeightUnits);
    }
}
