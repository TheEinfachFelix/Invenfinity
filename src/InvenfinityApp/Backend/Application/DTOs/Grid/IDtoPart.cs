using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid
{
    public interface IDtoPart: IDtoDropdownElement
    {
        public string? ColorTag { get; }
    }
}
