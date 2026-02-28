using Backend.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTOs
{
    public class DTOTreeGrid: IDotTreeItem
    {
        internal DTOTreeGrid(string Name, int Id, string path)
        {
            this.Name = Name;
            this.Id = Id;
            this.path = path;
        }

        public string Name { get; }
        public int Id { get; }
        public string path { get; }
        public IEnumerable<IDotTreeItem>? Children => null;
    }
}
