using Backend.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Location
{
    public class DTOTreeEditLocation : IDotTreeEditItem
    {
        internal DTOTreeEditLocation(string Name, int Id, int? ParentId, bool IsParentEditable, bool isDeletable)
        {
            this.Name = Name;
            this.Id = Id;
            this.ParentId = ParentId;
            this.IsParentEditable = IsParentEditable;
            this.isDeletable = isDeletable;
        }

        public string Name { get; set; }
        public int Id { get; set; }
        public string Type { get; set; } = "Location";
        public int? ParentId { get; set; }
        public bool IsParentEditable { get; }


        public int Xsize { get; set; } = 0;
        public int Ysize { get; set; } = 0;
        public int XminVal { get; } = 0;
        public int YminVal { get; } = 0;
        public bool XYisEditable { get; } = false;

        public bool isDeletable { get; }

    }
    
}
