using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain;
using Backend.Infrastructure.Datenbank;
using DBconnector;

namespace Backend.Application.UseCases
{
    public class UcRoot
    {
        internal Dset Data;
        internal RepoDatabase Repo;
        internal AppDbContext dbContext;
        public UcLocations Locations;
        public UcGrids Grid;
        public UcBins Bins;
        public UcRoot()
        {
            dbContext = new AppDbContext();
            Repo = new RepoDatabase(dbContext);
            Data = Repo.GetData();
            Locations = new UcLocations(Repo, Data);
            Grid = new UcGrids(Repo, Data);
            Bins = new UcBins(Repo, Data);

        }

    }
}
