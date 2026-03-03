using Backend.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTOs
{
    public class DTOTreeLocation: IDotTreeItem
    {
        internal DTOTreeLocation(string Name, int id, string path)
        {
            this.Name = Name;
            Id = id;
            this.Path = path;
        }
        public string Name {  get; }
        public int Id { get; }
        public string Path { get; }
        public List<IDotTreeItem> Children { get; set; } = new List<IDotTreeItem>();
        IEnumerable<IDotTreeItem> IDotTreeItem.Children => Children;
    }
}
