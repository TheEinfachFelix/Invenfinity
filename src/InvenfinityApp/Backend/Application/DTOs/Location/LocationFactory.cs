using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain;

namespace Backend.Application.DTOs.Location
{
    internal static class LocationFactory
    {
        public static DTOTreeLocation CreateLocation (DLocation root)
        {
            var outp = CreateLocationSingle(root);
            foreach (var item in root.Children)
            {
                outp.Children.Add(CreateLocation(item));
            }
            foreach (var item in root.Grids)
            {
                outp.Children.Add(CreateGrid(item));
            }
            return outp;
        }

        public static DTOTreeGrid CreateGrid(DGrid grid)
        {
            var gridSize = grid.GetMinRequiredGridSize();
            return new DTOTreeGrid  
                (grid.Name, grid.GridId, grid.GetPath(), grid.LocationId, grid.Xmax, grid.Ymax, gridSize.Xpos, gridSize.Ypos, grid.isDeletable());
        }

        public static DTOTreeLocation CreateLocationSingle(DLocation location)
        {
            var isParentEditable = location.LocationId != 1;
            return new DTOTreeLocation
                (location.Name, location.LocationId, location.GetPath(), location.ParentId,isParentEditable, location.isDeletable());
        }

        public static List<DTOTreeGrid> CreateGridList(List<DGrid> inList)
        {
            List<DTOTreeGrid> outp = new List<DTOTreeGrid>();
            foreach (var grid in inList)
            {
                outp.Add(CreateGrid(grid));
            }
            return outp;
        }
    }
}
