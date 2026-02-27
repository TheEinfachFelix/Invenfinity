using Backend.Application.UseCases;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Location.AddGrid(this);

            Grid = createGrid();
        }

        internal List<List<DBin?>> createGrid()
        {
            var outp = new List<List<DBin?>>();
            for (int i = 0; i < Xmax; i++)
            {
                outp.Add(new List<DBin?>());
                for (int j = 0; j < Ymax; j++)
                {
                    outp[i].Add(null);
                }
            }
            return outp;
        }
        public int GridId { get; }

        public int? LocationId { get; }

        public string Name { get; }

        public int Xmax { get; set; }

        public int Ymax { get; set; }

        public virtual DLocation Location { get; }

        public List<List<DBin?>> Grid { get; set; }

        public BinPos GetBinPosInGrid(DBin inBin)
        {
            var pos = GetAllBinPosInGrid(inBin);
            if (pos.Count == 0)
                throw new Exception("Bin not found in grid");
            return pos[0];
        }

        public List<BinPos> GetAllBinPosInGrid(DBin inBin)
        {
            var outp = new List<BinPos>();
            for (int X = 0; X < Xmax; X++)
            {
                for (int Y = 0; Y < Ymax; Y++)
                {
                    var bin = Grid[X][Y];
                    if (bin != null && bin.BinId == inBin.BinId)
                    {
                        outp.Add(new(X, Y));
                    }
                }
            }
            return outp;
        }

        public List<DBin> GetAllBinsInGrid()
        {
            var outp = new List<DBin>();
            for (int X = 0; X < Xmax; X++)
            {
                for (int Y = 0; Y < Ymax; Y++)
                {
                    var bin = Grid[X][Y];
                    if (bin != null && !outp.Any(b => b.BinId == bin.BinId))
                    {
                        outp.Add(bin);
                    }
                }
            }
            return outp;
        }

        public void RemoveBin(DBin inBin)
        {
            for (int X = 0; X < Xmax; X++)
            {
                for (int Y = 0; Y < Ymax; Y++)
                {
                    var bin = Grid[X][Y];
                    if (bin != null && bin.BinId == inBin.BinId)
                    {
                        Grid[X][Y] = null;
                    }
                }
            }
            inBin.Grid = null;
        }
        public void AddBin(DBin inBin, int X, int Y)
        {
            var width = inBin.BinType.X;
            var height = inBin.BinType.Y;
            for (int Xoffset = 0; Xoffset < width; Xoffset++)
            {
                for (int Yoffset = 0; Yoffset < height; Yoffset++)
                {
                    if (Grid[X + Xoffset][Y + Yoffset] != null) throw new Exception("Grid Pos already filled");
                    Grid[X + Xoffset][Y + Yoffset] = inBin;
                }
            }
            inBin.Grid = this;
        }

        public void MoveBin(DBin inBin, int newX, int newY)
        {
            var pos = inBin.GetPos();
            if (pos is null) throw new Exception("Bin not in grid");
            RemoveBin(inBin);
            AddBin(inBin, newX, newY);
        }
        public bool IsAreaFree(int X, int Y, DBinType inBinType, int binID)
        {
            if (X + inBinType.X > Xmax || Y + inBinType.Y > Ymax) return false;
            if (X < 0 || Y < 0) return false;
            var width = inBinType.X;
            var height = inBinType.Y;
            for (int Xoffset = 0; Xoffset < width; Xoffset++)
            {
                for (int Yoffset = 0; Yoffset < height; Yoffset++)
                {
                    var gridpos = Grid[X + Xoffset][Y + Yoffset];
                    if (gridpos != null && gridpos.BinId != binID) return false;
                }
            }
            return true;
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
