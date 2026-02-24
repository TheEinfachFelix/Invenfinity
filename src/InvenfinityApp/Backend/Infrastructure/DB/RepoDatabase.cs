using Backend.Domain;
using Backend.Infrastructure.Mapper;
using DBconnector;
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

            var inloc = context.Locations.Single(l => l.LocationId == 1);
            LoadGridsRecursively(inloc);




            if (inloc == null) throw new Exception("The root Location was not found");
            outp.Root = DBMapper.mapLocation(inloc, null, outp);
            return outp;
        }

        void LoadGridsRecursively(Location loc)
        {
            // Grids und GridPos + Bin + BinType + BinSlots + Part laden
            context.Entry(loc)
                .Collection(l => l.Grids)
                .Query()
                .Include(g => g.GridPos)
                    .ThenInclude(gp => gp.Bin)
                        .ThenInclude(b => b.BinType)
                .Include(g => g.GridPos)
                    .ThenInclude(gp => gp.Bin)
                        .ThenInclude(b => b.BinSlots)
                            .ThenInclude(bs => bs.Part)
                .Load();

            // alle Kinder laden
            context.Entry(loc)
                .Collection(l => l.InverseMasterLocation)
                .Load();

            foreach (var child in loc.InverseMasterLocation)
            {
                LoadGridsRecursively(child);
            }
        }
    }
}
