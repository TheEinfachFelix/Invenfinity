using LabelMaker.Models.Label;
using LabelMaker.Services;
using LabelMaker.Templates.Json;
using LabelMakerWPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMakerWPF.Models.Label.Elements
{
    internal class LabelElementPart : LabelElementBase, ILabelElement
    {
        private List<ILabelElement> Elements;
        public static string Name { get; } = "partElement";

        public LabelElementPart(List<ILabelElement> list, double padding, double minScale, double maxScale, HorisontalAlignCases hori, VerticalAlignCases vert, OrientationCases orient) : base(padding, minScale, maxScale, hori, vert, orient)
        {
            this.Elements = list;

            // Scale berechnen
            double standardLen = RenderStandardSize(12).Bounds.Width;
            double minLen = 0;
            double maxLen = 0;
            foreach (var item in Elements)
            {
                double itemLen = item.RenderStandardSize(12).Bounds.Width;
                minLen += itemLen * item.MinScale;
                maxLen += itemLen * item.MaxScale;
            }
            double newMinScale = minLen / standardLen;
            double newMaxScale = maxLen / standardLen;
            if (MinScale == new LayoutItem().minScale || newMinScale > minScale)
                this.MinScale = Math.Max(this.MinScale, newMinScale);
            if (MaxScale == new LayoutItem().maxScale || newMaxScale < maxScale)
                this.MaxScale = Math.Max(this.MaxScale, newMaxScale);
        }

        public override DrawingGroup Render(double labelHeightUnits, double labelLengthUnits)
        {
            var data = LayoutHelper.RenderAndScaleList(Elements, labelHeightUnits, labelLengthUnits);
            return LayoutHelper.CreateDrawGroup(data, this, labelLengthUnits, labelHeightUnits);
        }

        public override DrawingGroup RenderStandardSize(double labelHeightUnits)
        {
            var newlist = new List<DrawingGroup>();

            foreach (var item in Elements)
            {
                newlist.Add(item.RenderStandardSize(labelHeightUnits));
            }
            return LayoutHelper.concadGroups(newlist);
        }

        public static LabelElementPart GenerateElement(LayoutItem item, List<ILabelElement> list)
        {
            HorisontalAlignCases hori = Enum.Parse<HorisontalAlignCases>(item.horisontalAlign, true);
            VerticalAlignCases vert = Enum.Parse<VerticalAlignCases>(item.verticalAlign, true);
            OrientationCases orient = Enum.Parse<OrientationCases>(item.orientation, true);
            return new(list, item.padding, item.minScale, item.maxScale, hori, vert, orient);
        }
    }
}
