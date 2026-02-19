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
        public DTOLocation GetLocations () 
        { 
            return LocationFactory.CreateLocation(_root.Data.Root); 
        }
    }
}
