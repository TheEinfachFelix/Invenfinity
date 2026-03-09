using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LabelMaker.Models.Label.Elements
{
    internal class LabelElementText : ILabelElement
    {
        public LabelElementText(string text, int? widthMm, int? padding) 
        { 
            this.Text = text;
            this.WidthMm = widthMm;
            this.Padding = padding;

        }

        public string Text { get; private set; }
        public int? WidthMm { get; private set; }
        public int? Padding { get; private set; }
        public static string Name => "text";
    }
}
