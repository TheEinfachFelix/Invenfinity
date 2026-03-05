using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid.Edit
{
    public class DTOEditBinType : IDtoDropdownElement
    {
        internal DTOEditBinType(int Id, int SlotCount, int xSize, int ySize) 
        { 
            this.Id = Id;
            this.SlotCount = SlotCount;
            this.xSize = xSize;
            this.ySize = ySize;
        }
        public int Id { get; }
        public string Name { get { return xSize.ToString() + "x" + ySize.ToString() + " Slots:" + SlotCount.ToString(); } }
        public int SlotCount { get; }
        public int xSize { get; }
        public int ySize { get; }
    }
}
