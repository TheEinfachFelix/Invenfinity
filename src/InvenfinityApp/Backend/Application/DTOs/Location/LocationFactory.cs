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
            var outp = new DTOTreeLocation(root.Name, root.LocationId);
            foreach (var item in root.Childeren)
            {
                outp.Children.Add(CreateLocation(item));
            }
            foreach (var item in root.Grids)
            {
                outp.Children.Add(new DTOTreeGrid(item.Name, item.GridId));
            }
            return outp;
        }
    }
}
