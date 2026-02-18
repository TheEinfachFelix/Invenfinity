using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTOs
{
    public class DTOLocation: IDotTreeItem
    {
        public string name {  get; }
        public List<IDotTreeItem> Children { get; }

    }
}
