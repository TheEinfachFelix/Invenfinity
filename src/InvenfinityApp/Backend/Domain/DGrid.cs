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

        public DGrid(int GridID, string Name, DLocation Location, int Xmax, int Ymax)
        {
            this.GridId = GridID;
            this.Name = Name;
            this.Location = Location;
            LocationId = Location.LocationId;
            this.Xmax = Xmax;
            this.Ymax = Ymax;

            Location.AddGrid(this);

            _grid = CreateGridMatrix();
        }

        internal List<List<DBin?>> CreateGridMatrix()
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

        public int? LocationId { get; set; }

        public string Name { get; set; }

        public int Xmax { get; private set; }

        public int Ymax { get; private set; }

        public virtual DLocation Location { get; }

        private List<List<DBin?>> _grid;
        public IReadOnlyList<IReadOnlyList<DBin?>> Grid => _grid;

        internal IEnumerable<(int x, int y)> AllPositions()
        {
            for (int x = 0; x < Xmax; x++)
            {
                for (int y = 0; y < Ymax; y++)
                {
                    yield return (x, y);
                }
            }
        }

        public List<DBin> GetAllBinsInGrid()
        {
            var set = new HashSet<int>();
            var result = new List<DBin>();

            foreach (var (x, y) in AllPositions())
            {
                var bin = Grid[x][y];
                if (bin != null && set.Add(bin.BinId))
                    result.Add(bin);
            }
            return result;
        }
        public DBin? FindBinByID(int id) 
        { 
            var bins = GetAllBinsInGrid();
            var bin = bins.FirstOrDefault(b => b.BinId == id);
            return bin;
        }

        public void RemoveBin(DBin inBin)
        {
            foreach (var (x, y) in AllPositions())
            {
                var bin = Grid[x][y];
                if (bin != null && bin.BinId == inBin.BinId)
                {
                    _grid[x][y] = null;
                }
            }
            inBin.Grid = null;
        }
        public void AddBin(DBin inBin, int X, int Y)
        {
            if (!IsAreaFree(X, Y, inBin.BinType, inBin.BinId))
                throw new InvalidOperationException("Area not free");
            var width = inBin.BinType.X;
            var height = inBin.BinType.Y;
            for (int Xoffset = 0; Xoffset < width; Xoffset++)
            {
                for (int Yoffset = 0; Yoffset < height; Yoffset++)
                {
                    if (Grid[X + Xoffset][Y + Yoffset] != null) throw new Exception("Grid Pos already filled");
                    _grid[X + Xoffset][Y + Yoffset] = inBin;
                }
            }
            inBin.Grid = this;
        }
        public void AddBin(DBin inBin)
        {
            for (int x = 0; x <= Xmax - inBin.BinType.X; x++)
            {
                for (int y = 0; y <= Ymax - inBin.BinType.Y; y++)
                {
                    if (IsAreaFree(x, y, inBin.BinType, inBin.BinId))
                    {
                        AddBin(inBin, x, y);
                        return;
                    }
                }
            }
            throw new InvalidOperationException("No space for bin");
        }

        public void MoveBin(DBin inBin, int newX, int newY)
        {
            if (!IsAreaFree(newX, newY, inBin.BinType, inBin.BinId))
                throw new InvalidOperationException("Area not free");

            var oldPositions = inBin.GetAllPositions();

            RemoveBin(inBin);

            try
            {
                AddBin(inBin, newX, newY);
            }
            catch
            {
                // rollback
                foreach (var pos in oldPositions)
                    _grid[pos.Xpos][pos.Ypos] = inBin;

                throw;
            }
        }
        public bool IsAreaFree(int X, int Y, DBinType inBinType, int? binID)
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
        public BinPos GetMinRequiredGridSize()
        {
            var outp = new BinPos(0, 0);
            foreach (var (x, y) in AllPositions())
            {
                var bin = Grid[x][y];
                if (bin != null)
                {
                    if(outp.Xpos < x) outp.Xpos = x;
                    if(outp.Ypos < y) outp.Ypos = y;
                }
            }
            outp.Xpos++;
            outp.Ypos++;
            return outp;
        }
        public void ResizeGrid(int newXmax, int newYmax)
        {
            var required = GetMinRequiredGridSize();
            if (required.Xpos > newXmax || required.Ypos > newYmax)
                throw new InvalidOperationException("Grid too small for existing bins");
            Xmax = newXmax;
            Ymax = newYmax;
            var newGrid = CreateGridMatrix();

            for (int x = 0; x < Math.Min(Grid.Count, newXmax); x++)
            {
                for (int y = 0; y < Math.Min(Grid[x].Count, newYmax); y++)
                {
                    newGrid[x][y] = Grid[x][y];
                }
            }
            _grid = newGrid;
        }
        public BinPos? FindFreePosForBin(DBinType inBinType)
        {
            for (int x = Xmax - inBinType.X; x >= 0; x--)
            {
                for (int y = Ymax - inBinType.Y; y >= 0; y--)
                {
                    if (IsAreaFree(x, y, inBinType, null))
                        return new BinPos(x, y);
                }
            }
            return null;
        }

        public bool HasSpaceForBin(DBinType inBinType)
        {
            return FindFreePosForBin(inBinType) != null;
        }
        public bool isDeletable()
        {
            return GetAllBinsInGrid().Count == 0;
        }
        public string GetPath()
        {
            return Location.GetPath() + "/" + Name;
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
