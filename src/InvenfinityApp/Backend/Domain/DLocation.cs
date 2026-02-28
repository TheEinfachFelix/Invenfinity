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
        public DLocation(int LocationId, string Name, DLocation? Parent)
        {
            this.LocationId = LocationId;
            this.Name = Name;
            this.ParentId = null;
            if (Parent != null)
                this.ParentId = Parent.LocationId;
            this.Parent = Parent;

            if (Parent != null)
                Parent.AddChild(this);
        }
        public int LocationId { get; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public virtual ICollection<DGrid> Grids { get; set; } = new List<DGrid>();

        public virtual ICollection<DLocation> Childeren { get; set; } = new List<DLocation>();

        public virtual DLocation? Parent { get; }

        public DLocation? getLocationByID (int id)
        {
            if (LocationId == id) return this;  
            foreach (var child in Childeren)
            {
                var res = child.getLocationByID(id);
                if (res != null) return res;
            }
            return null;
        }

        public DGrid? getGridByID(int id)
        {
            foreach (var grid in Grids)
            {
                if (grid.GridId == id) return grid;
            }
            foreach (var child in Childeren)
            {
                var res = child.getGridByID(id);
                if (res != null) return res;
            }
            return null;
        }

        public void AddGrid(DGrid inGrid)
        {
            if (Grids.Contains(inGrid)) throw new Exception("Grid already in location");
            Grids.Add(inGrid);
        }
        internal void AddChild(DLocation inLoc)
        {
            if (Childeren.Contains(inLoc)) throw new Exception("Location already a child");
            Childeren.Add(inLoc);
        }

        public bool isDeletable()
        {
            return Grids.Count == 0 && Childeren.Count == 0 && LocationId != 1;
        }
    }
}
