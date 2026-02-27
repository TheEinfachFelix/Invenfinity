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
                outp.Children.Add(CreateLocation(item, outp.path+"/"+item.Name));
            }
            foreach (var item in root.Grids)
            {
                outp.Children.Add(new DTOTreeGrid(item.Name, item.GridId, outp.path+"/"+item.Name));
            }
            return outp;
        }
    }
}
