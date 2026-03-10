using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMaker.Templates.Json
{
    internal class Element
    {
        public string type { get; set; }
        public string value { get; set; }
        public int? minWidthMm { get; set; }
        public double? padding { get; set; }
        public double? minScale { get; set; }
        public double maxScale { get; set; } = 1;
        public string? splitChar { get; set; }
    }

    internal class JsonTemplate
    {
        public int version { get; set; }
        public List<Element> elements { get; set; }
    }
}
