using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMakerWPF.Templates.Printer
{
    public class PrinterPTouchP700 : IPrinter
    {
        public double XOffset => 8;

        public double YOffset => 4;

        public double MaxYSize => 10;
    }
}
