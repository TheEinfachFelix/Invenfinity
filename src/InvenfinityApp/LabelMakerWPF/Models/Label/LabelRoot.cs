using LabelMaker.Services;
using LabelMakerWPF.Services;
using LabelMakerWPF.Templates.Printer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMaker.Models.Label
{
    internal class LabelRoot
    {
        public double labelLength { get; set; }
        public double labelLengthUnits {  get { return Converter.mmtoUnits(labelLength); } }
        public List<ILabelElement> elements { get; set; } = [];
        public LabelRoot(double labelLength) 
        {
            this.labelLength = labelLength;
        }

        public DrawingGroup BuildVector(double labelHeightUnits)
        {

            return LayoutHelper.RenderAndScaleList(elements, labelHeightUnits, labelLengthUnits);




            // TODO
            // Stackpanel Implementation
            // Fixed Length Implementation
            // Layout umd Partimplementation



        }
    }
}
