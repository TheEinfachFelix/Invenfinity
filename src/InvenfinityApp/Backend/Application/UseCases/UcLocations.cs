using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.DTOs;
using Backend.Application.DTOs.Location;
using Backend.Infrastructure.Datenbank;
using DBconnector.Models;

namespace Backend.Application.UseCases
{
    public class UcLocations
    {
        private readonly UcRoot _root;
        internal UcLocations(UcRoot root)
        {
            _root = root;
        }
        public DTOTreeLocation GetLocations()
        {
            return LocationFactory.CreateLocation(_root.Data.Root);
        }

        public DTOTreeItemEdit GetEditItem(int id, string type)
        {
            if (type.Equals(typeof(DTOTreeLocation).Name))
            {

                var loc = _root.Data.Root.getLocationByID(id);
                if (loc == null) throw new Exception("Location not found");
                return LocationFactory.CreateEditItem(loc);
            }
            else if (type.Equals(typeof(DTOTreeGrid).Name))
            {
                var grid = _root.Data.Root.getGridByID(id);
                if (grid == null) throw new Exception("Grid not found");
                return LocationFactory.CreateEditItem(grid);
            }
            else
            {
                throw new Exception("Invalid type");
            }
        }
    }
}
