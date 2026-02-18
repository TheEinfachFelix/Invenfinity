using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain
{
    internal class Dset
    {
        public List<DPart> Parts { get; set; }
        public List<DBinType> Types { get; set; }
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
    }
}
