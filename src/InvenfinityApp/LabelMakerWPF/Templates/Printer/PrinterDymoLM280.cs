using LabelMaker.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMakerWPF.Templates.Printer
{
    public class PrinterDymoLM280 : IPrinter
    {
        public double XOffset => Converter.mmtoUnits(11);

        public double YOffset => 7.7;

        public double MaxYSize => 7.7;
    }
}
