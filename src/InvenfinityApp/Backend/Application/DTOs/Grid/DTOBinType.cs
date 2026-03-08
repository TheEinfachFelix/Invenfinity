using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid
{
    public class DTOBinType : IDtoDropdownElement
    {
        internal DTOBinType(int Id, int SlotCount, int xSize, int ySize) 
        { 
            this.Id = Id;
            this.SlotCount = SlotCount;
            this.XSize = xSize;
            this.YSize = ySize;
        }
        public int Id { get; }
        public string Name => $"{XSize}x{YSize} Slots:{SlotCount}";
        public int SlotCount { get; }
        public int XSize { get; }
        public int YSize { get; }
    }
}
