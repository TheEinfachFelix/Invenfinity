using Backend.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Location
{
    public interface IDotTreeEditItem
    {
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
