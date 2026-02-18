using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.DTOs;

namespace Backend.Application.UseCases
{
    public class Locations
    {
        public Locations() { }
        public DTOLocation GetLocations () { return new DTOLocation(); }
    }
}
