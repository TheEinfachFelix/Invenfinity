using LabelMaker.Models.Bin;
using LabelMaker.Models.Label;
using LabelMaker.Models.Label.Elements;
using LabelMaker.Models.Part;
using LabelMaker.Services;
using LabelMaker.Templates.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Media;

namespace LabelMakerWPF.Models.Label.Elements
{
    internal class LabelElementStack : LabelElementBase, ILabelElement
    {
        public List<ILabelElement> top = [];
        public List<ILabelElement> btm = [];
        public LabelElementStack(double padding, double minScale, double maxScale, List<ILabelElement> top, List<ILabelElement> btm, HorisontalAlignCases hori, VerticalAlignCases vert, OrientationCases orient)
            : base(padding, minScale, maxScale, hori, vert, orient)
        {
            this.top = top;
            this.btm = btm;
        }

        public static string Name => "stack";

        public override DrawingGroup Render(double labelHeightUnits, double labelLengthUnits)
        {
            throw new NotImplementedException();
        }

        public override DrawingGroup RenderStandardSize(double labelHeightUnits)
        {
            throw new NotImplementedException();
        }
        public static LabelElementStack GenerateElement(LayoutItem item, BinDataModel bin, PartDataModel part, string assetPath)
        {
            HorisontalAlignCases hori = Enum.Parse<HorisontalAlignCases>(item.horisontalAlign, true);
            VerticalAlignCases vert = Enum.Parse<VerticalAlignCases>(item.verticalAlign, true);
            OrientationCases orient = Enum.Parse<OrientationCases>(item.orientation, true);
            List<ILabelElement> top = Converter.toLabelElements(item.Top, bin, part, assetPath);
            List<ILabelElement> btm = Converter.toLabelElements(item.Bottom, bin, part, assetPath);
            return new(item.padding, item.minScale, item.maxScale, top, btm, hori, vert, orient);
        }


    }
}
