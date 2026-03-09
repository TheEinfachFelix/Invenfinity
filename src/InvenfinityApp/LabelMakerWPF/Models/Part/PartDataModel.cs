using System;
using System.Collections.Generic;
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
        
    }
}
