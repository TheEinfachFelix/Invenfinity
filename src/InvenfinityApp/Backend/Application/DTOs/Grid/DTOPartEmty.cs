using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs
{
    public class DTOPartEmty : IDotPart
    {
        public int PartId { get; } = int.MaxValue;
        public string PartName { get; } = "Leer";
        public string? ColorTag { get; } = "red";
    }
}
