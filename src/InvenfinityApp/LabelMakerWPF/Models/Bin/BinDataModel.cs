using LabelMaker.Models.Part;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMaker.Models.Bin
{
    public class BinDataModel
    {
        public int UnitLength { get; set; }
        public int SlotCount { get; set; }
        public double TotalLableLength //mm
        {
            get => UnitLength*42-2;
        }
        public double SlotLableLength
        {
            get => TotalLableLength/SlotCount;
        }
        public List<PartDataModel> Parts { get; set; } = [];
        public double Padding => 3;
    }
}
