using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMaker.Models.Label.Elements
{
    internal class LabelElementImage : ILabelElement
    {
        public LabelElementImage(string path,string type, string name, int? widthMm, int? padding)
        {
            this.path = path;
            this.type = type;
            this.name = name;
            this.WidthMm = widthMm;
            this.Padding = padding;

        }

        private string path;
        private string type;
        private string name;
        public int? WidthMm { get; private set; }
        public int? Padding { get; private set; }
        public static string Name => "image";

    }
}
