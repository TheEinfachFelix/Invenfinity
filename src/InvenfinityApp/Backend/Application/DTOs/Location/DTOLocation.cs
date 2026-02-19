using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTOs
{
    public class DTOLocation: IDotTreeItem
    {
        public DTOLocation(string Name, int id)
        {
            this.Name = Name;
            Id = id;
        }
        public string Name {  get; }
        public int Id { get; }
        public bool IsSelectable => false;
        public List<IDotTreeItem> Children { get; set; } = new List<IDotTreeItem>();

    }
}
