using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMaker.Models.Label.Elements
{
    internal class LabelElementQrCode : ILabelElement
    {
        public LabelElementQrCode(string  value, int? widthMm, int? padding)
        {
            this.value = value;
            this.WidthMm = widthMm;
            this.Padding = padding;

        }
        public string value;
        public int? WidthMm { get; private set; }
        public int? Padding { get; private set; }
        public static string Name => "qrcode";

    }
}
