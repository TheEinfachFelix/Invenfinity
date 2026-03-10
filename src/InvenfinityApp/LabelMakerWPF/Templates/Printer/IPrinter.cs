using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMakerWPF.Templates.Printer
{
    public interface IPrinter
    {
        public double XOffset { get; }
        public double YOffset { get; }
        public double MaxYSize { get; }
    }
}
