using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs
{
    public class DTOPartEmty : IDotPart
    {
        public int Id { get; } = int.MaxValue;
        public string Name { get; } = "Leer";
        public string? ColorTag { get; } = "red";
    }
}
