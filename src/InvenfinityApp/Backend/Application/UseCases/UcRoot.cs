using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain;
using Backend.Infrastructure.Datenbank;
using DBconnector.Models;

namespace Backend.Application.UseCases
{
    public class UcRoot
    {
        internal Dset Data;
        internal RepoDatabase RepoDatabase;
        internal AppDbContext dbContext;
        public UcLocations Locations;
        public UcRoot()
        {
            dbContext = new AppDbContext();
            RepoDatabase = new RepoDatabase(dbContext);
            Data = RepoDatabase.GetData();
            Locations = new UcLocations(this);
        }
        
    }
}
