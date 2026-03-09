using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMaker.Templates.Json
{
    internal class Element
    {
        public string type { get; set; }
        public string value { get; set; }
        public int? widthMm { get; set; }
        public int? padding { get; set; }
    }

    internal class JsonTemplate
    {
        public int version { get; set; }
        public List<string> data { get; set; }
        public List<Element> elements { get; set; }
    }
}
