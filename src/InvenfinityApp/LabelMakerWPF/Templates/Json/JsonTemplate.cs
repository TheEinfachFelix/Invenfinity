using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMaker.Templates.Json
{

    public class LayoutItem
    {
        public string type { get; set; }
        public double padding { get; set; }
        public string horisontalAlign { get; set; }
        public List<LayoutItem> Top { get; set; }
        public List<LayoutItem> Bottom { get; set; }
        public List<LayoutItem> elements { get; set; }
        public string value { get; set; }
        public double minScale { get; set; } = 0.5;
        public string verticalAlign { get; set; }
        public double maxScale { get; set; } = 1;
        public string orientation { get; set; }
    }

    public class Requirements
    {
        public int minWidthPerPart { get; set; }
        public string AssetType { get; set; }
    }

    public class JsonTemplate
    {
        public int version { get; set; }
        public Requirements requirements { get; set; }
        public List<LayoutItem> Layout { get; set; }
        public List<LayoutItem> partElement { get; set; }
    }

}
