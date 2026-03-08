using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs
{
    public sealed class DTOPartEmpty : IDotPart
    {
        public static readonly DTOPartEmpty Instance = new();

        private DTOPartEmpty() { }

        public int Id => int.MaxValue;
        public string Name => "Leer";
        public string? ColorTag => "red";
    }
}
