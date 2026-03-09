using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMaker.Models.Label
{
    internal interface ILabelElement
    {
        public int? WidthMm { get; }
        public int? Padding { get; }
        public static string Name { get; }
    }
}
