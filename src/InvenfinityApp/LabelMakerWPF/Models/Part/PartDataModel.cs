using LabelMakerWPF.Models.Part;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LabelMaker.Models.Part
{

    public class PartDataModel
    {
        public double Length { get; set; } // mm
        public int Id { get; set; }
        public ScrewThreadType Thread { get; set; }
        public ScrewDriveType Drive { get; set; }
        public ScrewHeadType Head { get; set; }
        public TemplateType Template { get; set; }
        public string GetTemplatePath(string assetPath)
        {
            return Path.Combine(assetPath, "Templates", Template.ToString() + ".json");
        }
    }
}
