using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid
{
    public interface IDtoPart: IDtoDropdownElement
    {
        public int Id { get; }
        public string? ColorTag { get; }
    }
}
