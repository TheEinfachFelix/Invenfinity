using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain;
using Backend.Infrastructure.Mapper;
using DBconnector.Models;

namespace Backend.Infrastructure.Datenbank
{
    public class RepoDatabase
    {
        AppDbContext context;
        public RepoDatabase(AppDbContext context) { this.context = context; }
        
        internal Dset GetData()
        {
            var outp = new Dset();

            foreach (var item in context.BinTypes.ToList())
            {
                outp.Types.Add(DBMapper.mapBinType(item));
            }
            foreach ( var item in context.Parts.ToList())
            {
                outp.Parts.Add(DBMapper.mapPart(item));
            }
            outp.Root = DBMapper.mapLocation(context.Locations.Where(x => x.LocationId == 1).FirstOrDefault(), null, outp);
            return outp;
        }
    }
}
