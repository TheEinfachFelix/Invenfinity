using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid.Edit
{
    public class DTOEditBin
    {
        internal DTOEditBin(int Id, int? GridId, List<DTOEditPart> Parts, DTOEditBinType BinType)
        {
            this.Id = Id;
            this.GridId = GridId;
            this.Parts = Parts;
            this.BinType = BinType;
        }
        public int Id { get; }
        public int? GridId { get; }
        public List<DTOEditPart> Parts { get; }
        public DTOEditBinType BinType { get; } 
    }
}
