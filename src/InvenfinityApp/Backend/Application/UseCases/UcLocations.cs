using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.DTOs;
using Backend.Application.DTOs.Location;
using Backend.Domain;
using Backend.Infrastructure.Datenbank;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Application.UseCases
{
    public class UcLocations
    {
        private readonly UcRoot _root;
        public UcLocationEdit Edit { get; private set; }
        internal UcLocations(UcRoot root)
        {
            _root = root;
            Edit = new UcLocationEdit(root);
        }

        public DTOTreeLocation GetLocations()
        {
            return _root.Data.Root.ToTreeDto();
        }


    }
}
