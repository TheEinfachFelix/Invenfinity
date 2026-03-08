using Backend.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTOs
{
    public class DTOTreeLocation : IDotTreeItem, IDotTreeEditItem
    {
        internal DTOTreeLocation(string Name, int Id, string path, int? ParentId, bool IsParentEditable, bool isDeletable)
        {
            this.Name = Name;
            this.Id = Id;
            this.Path = path;
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



        public string Path { get; }
        public List<IDotTreeItem> Children { get; set; } = new List<IDotTreeItem>();
        IEnumerable<IDotTreeItem> IDotTreeItem.Children => Children;
    }
}
