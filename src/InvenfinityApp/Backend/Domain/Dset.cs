using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain
{
    internal class Dset
    {
        public List<DPart> Parts { get; set; } = new List<DPart>();
        public List<DBinType> Types { get; set; } = new List<DBinType>();
        public DLocation Root {  get; set; }

        public DPart findPartbyID(int id)
        {
            foreach (var item in Parts)
            {
                if (item.PartId == id) return item;
            }
            throw new Exception("Part not found");
        }
        public DBinType findBinTypebyID(int id)
        {
            foreach (var item in Types)
            {
                if (item.BinTypeId == id) return item;
            }
            throw new Exception("BinType not found");
        }

        public DGrid findGridbyID(int id)
        {
            return findGridbyIDforSpecLoc(id, Root) ?? throw new Exception("Grid not found");

        }
        private DGrid? findGridbyIDforSpecLoc(int id, DLocation loc)
        {
            foreach (var item in loc.Grids)
            {
                if (item.GridId == id) return item;
            }
            foreach (var item in loc.Childeren)
            {
                var outp = findGridbyIDforSpecLoc(id, item);
                if (outp != null) return outp;
            }
            return null;
        }
        public DBin? findBininGrid(int id, DGrid grid)
        {
            foreach (var item in grid.Grid)
            {
                foreach (var bin in item)
                {
                    if (bin != null && bin.BinId == id) return bin;
                }
            }
            return null;
        }

    }
}
