using Backend.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTOs
{
    public class DTOTreeGrid: IDotTreeItem, IDtoDropdownElement, IDotTreeEditItem
    {
        internal DTOTreeGrid(string Name, int Id, string path, int? ParentId, int Xsize, int Ysize, int XminVal, int YminVal, bool isDeletable)
        {
            this.Name = Name;
            this.Id = Id;
            this.Path = path;
            this.ParentId = ParentId;
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

        public string Path { get; }
        public IEnumerable<IDotTreeItem>? Children => null;
    }
}
