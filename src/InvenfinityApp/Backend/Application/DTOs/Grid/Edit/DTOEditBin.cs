using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid.Edit
{
    public class DTOEditBin : IDtoDropdownElement
    {
        internal DTOEditBin(int Id, int? GridId, List<DTOPart> Parts, DTOEditBinType BinType, bool isDeletable)
        {
            this.Id = Id;
            this.GridId = GridId;
            this.Parts = Parts;
            this.BinType = BinType;
            this.isDeletable = isDeletable;
        }
        public string Name => Id.ToString();
        public int Id { get; }
        public int? GridId { get; }
        public List<DTOPart> Parts { get; }
        public DTOEditBinType BinType { get; } 
        public bool isDeletable { get; }
    }
}
