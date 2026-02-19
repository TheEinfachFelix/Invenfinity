using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTOs
{
    public class DTOGrid: IDotTreeItem
    {
        public DTOGrid(string Name, int Id)
        {
            this.Name = Name;
            this.Id = Id;
        }

        public string Name { get; }
        public int Id { get; }
        public bool IsSelectable => true;
    }
}
