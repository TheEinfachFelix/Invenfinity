using Backend.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Location
{
    public class DTOTreeItemEdit
    {
        internal DTOTreeItemEdit(string Name, int Id, string Type, int? ParentId, bool IsParentEditable, int Xsize, int Ysize, int XminVal, int YminVal, bool XYisEditable, bool isDeletable)
        {
            this.Name = Name;
            this.Id = Id;
            this.Type = Type;
            this.ParentId = ParentId;
            this.IsParentEditable = IsParentEditable;
            this.Xsize = Xsize;
            this.Ysize = Ysize;
            this.XminVal = XminVal;
            this.YminVal = YminVal;
            this.XYisEditable = XYisEditable;
            this.isDeletable = isDeletable;
        }

        public string Name { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ParentId { get; set; }
        public bool IsParentEditable { get; }
        

        public int Xsize { get; set; }
        public int Ysize { get; set; }
        public int XminVal { get; }
        public int YminVal { get; }
        public bool XYisEditable { get; }

        public bool isDeletable { get; }

    }
}
