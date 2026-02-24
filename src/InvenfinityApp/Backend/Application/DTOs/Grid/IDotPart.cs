using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs
{
    public interface IDotPart
    {
        public int PartId { get; }
        public string PartName { get; }
        public string? ColorTag { get; }
    }
}
