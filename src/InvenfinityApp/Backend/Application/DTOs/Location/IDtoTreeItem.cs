using Backend.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTOs.Location
{
    public interface IDtoTreeItem
    {
        public string Name { get; }
        public int Id { get; }
        public string Path { get; }
        public IEnumerable<IDtoTreeItem>? Children { get; }
    }
}
