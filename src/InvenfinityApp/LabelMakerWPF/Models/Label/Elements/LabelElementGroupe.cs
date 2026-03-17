using LabelMaker.Models.Bin;
using LabelMaker.Models.Label;
using LabelMaker.Models.Label.Elements;
using LabelMaker.Models.Part;
using LabelMaker.Services;
using LabelMaker.Templates.Json;
using LabelMakerWPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace LabelMakerWPF.Models.Label.Elements
{
    internal class LabelElementGroupe : LabelElementBase, ILabelElement
    {
        public List<ILabelElement> elements = [];
        public static string Name => "groupe";

        public LabelElementGroupe(double padding, double minScale, double maxScale, List<ILabelElement> elements, HorisontalAlignCases hori, VerticalAlignCases vert, OrientationCases orient)
            : base(padding, minScale, maxScale, hori, vert, orient)
        {
            this.elements = elements;

            // Scale berechnen
            double standardLen = RenderStandardSize(12).Bounds.Width;
            double minLen = 0;
            double maxLen = 0;
            foreach (var item in elements)
            {
                double itemLen = item.RenderStandardSize(12).Bounds.Width;
                minLen += itemLen * item.MinScale;
                maxLen += itemLen * item.MaxScale;
            }
            double newMinScale = minLen / standardLen;
            double newMaxScale = maxLen / standardLen;
            if (MinScale == new LayoutItem().minScale || newMinScale > minScale)
                minScale = newMinScale;
            if (MaxScale == new LayoutItem().maxScale || newMaxScale < maxScale)
                maxScale = newMaxScale;
        }
        public override DrawingGroup Render(double labelHeightUnits, double labelLengthUnits)
        {
            return RenderStandardSize(labelHeightUnits);
            // TODO
        }

        public override DrawingGroup RenderStandardSize(double labelHeightUnits)
        {
            var list = new List<DrawingGroup>();
            foreach (var item in elements)
            {
                list.Add(item.RenderStandardSize(labelHeightUnits));
            }
            return SvgHelper.concadGroups(list);
        }
        public static LabelElementGroupe GenerateElement(LayoutItem item, BinDataModel bin, PartDataModel part, string assetPath)
        {
            HorisontalAlignCases hori = Enum.Parse<HorisontalAlignCases>(item.horisontalAlign, true);
            VerticalAlignCases vert = Enum.Parse<VerticalAlignCases>(item.verticalAlign, true);
            OrientationCases orient = Enum.Parse<OrientationCases>(item.orientation, true);
            List<ILabelElement> list = Converter.toLabelElements(item.elements, bin, part, assetPath);
            return new(item.padding, item.minScale, item.maxScale, list, hori, vert, orient);
        }
    }
}
