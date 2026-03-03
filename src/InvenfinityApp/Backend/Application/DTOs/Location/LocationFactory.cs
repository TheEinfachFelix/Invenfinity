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
            var outp = new DTOTreeLocation(root.Name, root.LocationId, root.GetPath());
            foreach (var item in root.Children)
            {
                outp.Children.Add(CreateLocation(item));
            }
            foreach (var item in root.Grids)
            {
                outp.Children.Add(new DTOTreeGrid(item.Name, item.GridId, item.GetPath()));
            }
            return outp;
        }

        public static DTOTreeEditGrid CreateEditItem(DGrid grid)
        {
            var gridSize = grid.GetMinRequiredGridSize();
            return new DTOTreeEditGrid  
                (grid.Name, grid.GridId, grid.LocationId, grid.Xmax, grid.Ymax, gridSize.Xpos, gridSize.Ypos, grid.isDeletable());
        }

        public static DTOTreeEditLocation CreateEditItem(DLocation location)
        {
            var isParentEditable = location.LocationId != 1;
            return new DTOTreeEditLocation
                (location.Name, location.LocationId, location.ParentId,isParentEditable, location.isDeletable());
        }

        public static List<DTOTreeGrid> CreateGridList(List<DGrid> inList)
        {
            List<DTOTreeGrid> outp = new List<DTOTreeGrid>();
            foreach (var grid in inList)
            {
                outp.Add(new DTOTreeGrid(grid.Name, grid.GridId, grid.GetPath()));
            }
            return outp;
        }
    }
}
