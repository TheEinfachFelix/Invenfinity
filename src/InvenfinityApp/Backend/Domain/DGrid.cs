using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Backend.Domain
{
    internal class DGrid
    {

        public DGrid(int GridID, string Name, DLocation Location, int LocationID, int Xmax, int Ymax)
        {
            this.GridId = GridID;
            this.Name = Name;
            this.Location = Location;
            this.LocationId = LocationID;
            this.Xmax = Xmax;
            this.Ymax = Ymax;

            Location.Grids.Add( this );
        }
        public int GridId { get; }

        public int? LocationId { get; }

        public string Name { get; }

        public int Xmax { get; set; }

        public int Ymax { get; set; }

        public virtual DLocation Location { get; }

        public List<List<DBin?>> Grid { get; set; } = new List<List<DBin?>>();

        public BinPos GetGridPos(DBin inBin)
        {
            for (int X = 0; X < Xmax; X++)
            {
                for (int Y = 0; Y < Ymax; Y++)
                {
                    var bin = Grid[X][Y];
                    if (bin != null && bin.BinId == inBin.BinId)
                    {
                        return new(X,Y);
                    }
                }
            }
            throw new Exception("Bin not found in grid");
        }

    }

    internal record BinPos
    {
        public BinPos(int X, int Y)
        {
            Xpos = X;
            Ypos = Y;
        }
        public int Xpos;
        public int Ypos;
    }
}
