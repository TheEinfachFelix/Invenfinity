using Backend.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Location
{
    public class DTOTreeEditGrid: IDotTreeEditItem
    {
        internal DTOTreeEditGrid(string Name, int Id, int? ParentId, int Xsize, int Ysize, int XminVal, int YminVal, bool isDeletable)
        {
            this.Name = Name;
            this.Id = Id;
            this.ParentId = ParentId;
            this.IsParentEditable = IsParentEditable;
            this.Xsize = Xsize;
            this.Ysize = Ysize;
            this.XminVal = XminVal;
            this.YminVal = YminVal;
            this.isDeletable = isDeletable;
        }

        public string Name { get; set; }
        public int Id { get; set; }
        public string Type { get; set; } = "Grid";
        public int? ParentId { get; set; }
        public bool IsParentEditable { get; } = true;


        public int Xsize { get; set; }
        public int Ysize { get; set; }
        public int XminVal { get; }
        public int YminVal { get; }
        public bool XYisEditable { get; } = true;

        public bool isDeletable { get; }

    }

}
