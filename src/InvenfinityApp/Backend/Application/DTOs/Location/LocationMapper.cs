using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain;

namespace Backend.Application.DTOs.Location
{
    internal static class LocationMapper
    {
        public static DTOTreeLocation ToTreeDto (this DLocation root)
        {
            var outp = root.ToSingleTreeDto();
            foreach (var item in root.Children)
            {
                outp.Children.Add(item.ToTreeDto());
            }
            foreach (var item in root.Grids)
            {
                outp.Children.Add(item.ToTreeDto());
            }
            return outp;
        }
        public static DTOTreeLocation ToSingleTreeDto(this DLocation location)
        {
            var isParentEditable = location.LocationId != 1;
            return new DTOTreeLocation
                (location.Name, location.LocationId, location.GetPath(), location.ParentId, isParentEditable, location.isDeletable());
        }

        public static DTOTreeGrid ToTreeDto(this DGrid grid)
        {
            var gridSize = grid.GetMinRequiredGridSize();
            return new DTOTreeGrid  
                (grid.Name, grid.GridId, grid.GetPath(), grid.LocationId, grid.Xmax, grid.Ymax, gridSize.Xpos, gridSize.Ypos, grid.isDeletable());
        }

        public static List<DTOTreeGrid> ToTreeDto(this List<DGrid> inList)
        {
            List<DTOTreeGrid> outp = new List<DTOTreeGrid>();
            foreach (var grid in inList)
            {
                outp.Add(grid.ToTreeDto());
            }
            return outp;
        }
    }
}
