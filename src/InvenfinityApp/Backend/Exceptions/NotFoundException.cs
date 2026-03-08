using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, int? id)
            : base($"{entity} with id {id} not found")
        {
        }
    }
}
