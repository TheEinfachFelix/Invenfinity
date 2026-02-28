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
        public static DTOTreeLocation CreateLocation (DLocation root, string path = "")
        {
            var outp = new DTOTreeLocation(root.Name, root.LocationId, path+"/"+root.Name);
            foreach (var item in root.Childeren)
            {
                outp.Children.Add(CreateLocation(item, outp.path));
            }
            foreach (var item in root.Grids)
            {
                outp.Children.Add(new DTOTreeGrid(item.Name, item.GridId, outp.path+"/"+item.Name));
            }
            return outp;
        }

        public static DTOTreeItemEdit CreateEditItem(DGrid grid)
        {
            var gridSize = grid.getMinGridSize();
            return new DTOTreeItemEdit
                (grid.Name, grid.GridId, "Grid", grid.LocationId, true, grid.Xmax, grid.Ymax, gridSize.Xpos, gridSize.Ypos, true, grid.isDeletable());
        }

        public static DTOTreeItemEdit CreateEditItem(DLocation location)
        {
            var isParentEditable = location.LocationId != 1;
            return new DTOTreeItemEdit
                (location.Name, location.LocationId, "Location", location.ParentId,isParentEditable, 0, 0, 0, 0, false, location.isDeletable());
        }

    }
}
