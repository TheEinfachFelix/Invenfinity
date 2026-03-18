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
        public string Typename { get; set; }
        public ScrewThreadType Thread { get; set; }
    }
}
