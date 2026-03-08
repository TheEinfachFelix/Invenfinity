using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid
{
    public sealed class DTOPartEmpty : IDtoPart
    {
        public static readonly DTOPartEmpty Instance = new();

        private DTOPartEmpty() { }
        public int Id => -1;
        public int? DropdownId => null;
        public string Name => "Leer";
        public string? ColorTag => "red";
    }
}
