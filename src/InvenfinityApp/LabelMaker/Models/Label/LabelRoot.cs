using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMaker.Models.Label
{
    internal class LabelRoot
    {
        public double LabelLength { get; set; }
        public List<ILabelElement> Elements { get; set; } = new List<ILabelElement>();
        public LabelRoot(double labelLength) 
        {
            LabelLength = labelLength;
        }
    }
}
