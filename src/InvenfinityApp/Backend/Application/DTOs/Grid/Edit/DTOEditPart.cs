using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid.Edit
{
    public class DTOEditPart : IDtoDropdownElement
    {
        internal DTOEditPart(int Id)
        {
            this.Id = Id;
        }
        public int Id { get; }
        public string Name { get; } = "NotImplemented";
    }
}
