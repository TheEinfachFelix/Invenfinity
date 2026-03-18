using LabelMaker.Models.Bin;
using LabelMaker.Services;
using LabelMakerWPF.Services;
using LabelMakerWPF.Templates.Printer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMaker.Models.Label
{
    public class PartLabelRoot
    {
        internal double labelLength { get; set; }
        internal double labelLengthUnits {  get { return Converter.mmtoUnits(labelLength); } }
        internal List<ILabelElement> elements { get; set; } = [];
        internal PartLabelRoot(double labelLength) 
        {
            this.labelLength = labelLength;
        }

        internal DrawingGroup Render(double labelHeightUnits)
        {
            return LayoutHelper.RenderAndScaleList(elements, labelHeightUnits, labelLengthUnits);
        }
    }
}
