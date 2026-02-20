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
            //  Get the bin Types
            foreach (var item in context.BinTypes.ToList())
            {
                outp.Types.Add(DBMapper.mapBinType(item));
            }
            // get the parts
            foreach ( var item in context.Parts.ToList())
            {
                outp.Parts.Add(DBMapper.mapPart(item));
            }
            // get the rest of the Data
            var inloc = context.Locations.Where(x => x.LocationId == 1).FirstOrDefault();
            if (inloc == null) throw new Exception("The root Location was not found");
            outp.Root = DBMapper.mapLocation(inloc, null, outp);
            return outp;
        }
    }
}
