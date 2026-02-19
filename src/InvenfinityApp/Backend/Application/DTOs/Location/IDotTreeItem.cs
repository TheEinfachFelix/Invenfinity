using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTOs
{
    public interface IDotTreeItem
    {
        public string Name { get; }
        public int Id { get; }
        bool IsSelectable { get; }
    }
}
