using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain;

namespace Backend.Application.DTOs.Location
{
    internal class LocationFactory
    {
        public static DTOLocation CreateLocation (DLocation root)
        {
            var outp = new DTOLocation(root.Name, root.LocationId);
            foreach (var item in root.Childeren)
            {
                outp.Children.Add(CreateLocation(item));
            }
            foreach (var item in root.Grids)
            {
                outp.Children.Add(new DTOGrid(item.Name, item.GridId));
            }
            return outp;
        }
    }
}
