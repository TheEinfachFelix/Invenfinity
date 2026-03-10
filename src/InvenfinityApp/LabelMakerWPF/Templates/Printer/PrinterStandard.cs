using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMakerWPF.Templates.Printer
{
    internal class PrinterStandard : IPrinter
    {
        public double XOffset => 0;
        public double YOffset => 0;
        public double MaxYSize => double.MaxValue;

    }
}
