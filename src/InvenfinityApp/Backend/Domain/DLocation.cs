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

            if (Parent != null )
                Parent.Childeren.Add( this );
        }
        public int LocationId { get; }

        public string Name { get; }

        public int? ParentId { get; }

        public virtual ICollection<DGrid> Grids { get; set; }

        public virtual ICollection<DLocation> Childeren { get; set; } = new List<DLocation>();

        public virtual DLocation? Parent { get; }
    }
}
