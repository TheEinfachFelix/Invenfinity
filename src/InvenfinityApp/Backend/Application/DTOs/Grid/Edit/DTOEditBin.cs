using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid.Edit
{
    public class DTOEditBin : IDtoDropdownElement
    {
        internal DTOEditBin(int Id, int? GridId, List<DTOEditPart> Parts, DTOEditBinType BinType)
        {
            this.Id = Id;
            this.GridId = GridId;
            this.Parts = Parts;
            this.BinType = BinType;
        }
        public string Name => Id.ToString();
        public int Id { get; }
        public int? GridId { get; }
        public List<DTOEditPart> Parts { get; }
        public DTOEditBinType BinType { get; } 
    }
}
