using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs
{
    public interface IDotPart
    {
        public int Id { get; }
        public string Name { get; }
        public string? ColorTag { get; }
    }
}
