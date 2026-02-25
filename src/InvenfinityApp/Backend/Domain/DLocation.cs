using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain
{
    internal class DLocation
    {
        public DLocation(int LocationId, string Name, int? ParentID, DLocation? Parent)
        {
            this.LocationId = LocationId;
            this.Name = Name;
            this.ParentId = ParentID;
            this.Parent = Parent;

            if (Parent != null)
                Parent.AddChild(this);
        }
        public int LocationId { get; }

        public string Name { get; }

        public int? ParentId { get; }

        public virtual ICollection<DGrid> Grids { get; set; } = new List<DGrid>();

        public virtual ICollection<DLocation> Childeren { get; set; } = new List<DLocation>();

        public virtual DLocation? Parent { get; }

        public void AddGrid(DGrid inGrid)
        {
            if (Grids.Contains(inGrid)) throw new Exception("Grid already in location");
            Grids.Add(inGrid);
        }
        public void AddChild(DLocation inLoc)
        {
            if (Childeren.Contains(inLoc)) throw new Exception("Location already a child");
            Childeren.Add(inLoc);
        }
    }
}
