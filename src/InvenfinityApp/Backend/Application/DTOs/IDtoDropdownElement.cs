using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs
{
    public interface IDtoDropdownElement
    {
        public string Name { get; }
        public int? DropdownId { get; }
    }
}
