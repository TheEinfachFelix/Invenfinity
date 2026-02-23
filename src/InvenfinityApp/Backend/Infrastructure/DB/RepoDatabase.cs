using Backend.Domain;
using Backend.Infrastructure.Mapper;
using DBconnector.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

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


            var inloc = context.Locations
                        .Where(l => l.LocationId == 1)

                        // gesamte Location-Hierarchie
                        .Include(l => l.InverseMasterLocation)
                            .ThenInclude(l => l.InverseMasterLocation)

                        // Grids
                        .Include(l => l.Grids)
                            .ThenInclude(g => g.GridPos)

                                // Bin
                                .ThenInclude(gp => gp.Bin)

                                    // BinType
                                    .ThenInclude(b => b.BinType)

                        // BinSlots + Part
                        .Include(l => l.Grids)
                            .ThenInclude(g => g.GridPos)
                                .ThenInclude(gp => gp.Bin)
                                    .ThenInclude(b => b.BinSlots)
                                        .ThenInclude(bs => bs.Part)

                        .AsSplitQuery()   // wichtig gegen Cartesian Explosion
                        .SingleAsync().GetAwaiter().GetResult();




            if (inloc == null) throw new Exception("The root Location was not found");
            outp.Root = DBMapper.mapLocation(inloc, null, outp);
            return outp;
        }
    }
}
