using LabelMaker.Models.Bin;
using LabelMaker.Models.Label;
using LabelMaker.Models.Label.Elements;
using LabelMaker.Models.Part;
using LabelMaker.Services;
using LabelMaker.Templates.Json;
using LabelMakerWPF.Services;
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
            double rowHeigth = labelHeightUnits / 2;
            var newTop = LayoutHelper.RenderAndScaleList(top, rowHeigth, labelLengthUnits);
            var newBtm = LayoutHelper.RenderAndScaleList(btm, rowHeigth, labelLengthUnits);
            var neoTop = LayoutHelper.CreateDrawGroup(newTop, this, labelLengthUnits, rowHeigth);
            var neoBtm = LayoutHelper.CreateDrawGroup(newBtm, this, labelLengthUnits, rowHeigth);
            var stack = StackVertical(neoTop, neoBtm, rowHeigth);
            return LayoutHelper.CreateDrawGroup(stack, this, labelLengthUnits, labelHeightUnits);
            return StackVertical(neoTop, neoBtm, rowHeigth);
        }

        public override DrawingGroup RenderStandardSize(double labelHeightUnits)
        {
            double rowHeigth = labelHeightUnits / 2;

            var newTop = new List<DrawingGroup>();
            foreach (var item in top)
            {
                newTop.Add(item.RenderStandardSize(rowHeigth));
            }
            var neoTop = LayoutHelper.concadGroups(newTop);

            var newBtm = new List<DrawingGroup>();
            foreach (var item in btm)
            {
                newBtm.Add(item.RenderStandardSize(rowHeigth));
            }
            var neoBtm = LayoutHelper.concadGroups(newBtm);
            return StackVertical(neoTop, neoBtm, rowHeigth);
        }

        private DrawingGroup StackVertical(DrawingGroup topGroup, DrawingGroup bottomGroup, double offsetY)
        {
            var result = new DrawingGroup();

            // Top bleibt bei Y = 0
            result.Children.Add(topGroup);

            // Bottom nach unten verschieben
            var transformedBottom = new DrawingGroup();
            transformedBottom.Transform = new TranslateTransform(0, offsetY);
            transformedBottom.Children.Add(bottomGroup);

            result.Children.Add(transformedBottom);

            return result;
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
