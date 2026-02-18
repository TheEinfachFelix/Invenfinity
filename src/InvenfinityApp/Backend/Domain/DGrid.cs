using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBconnector.Models;

namespace Backend.Domain
{
    internal class DGrid
    {

        public DGrid(int GridID, string Name, DLocation Location, int LocationID)
        {
            this.GridId = GridID;
            this.Name = Name;
            this.Location = Location;
            this.LocationId = LocationID;

            Location.Grids.Add( this );
        }
        public int GridId { get; }

        public int? LocationId { get; }

        public string Name { get; }

        public virtual DLocation Location { get; }

        public List<List<DBin>> Grid { get; set; }
    }
}
