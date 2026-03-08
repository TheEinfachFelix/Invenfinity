using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Grid
{
    public class DTOPart : IDtoPart
    {
        internal DTOPart(int PartId)
        {
            this.Id = PartId;
        }

        public int Id { get; }
        public int? DropdownId => Id;
        public string Name { get; } = "Bauteil";



        // Label / PDF Bezug
        //public string? LabelTemplateId { get; }   // z.B. "gridfinity-part-small"
        //public string? GeneratedLabelPdfPath { get; } // optional Pfad/URI zur generierten PDF

        // Darstellungshilfen
        //public string? SmallIconUri { get; }      // Thumbnail / Icon für die UI
        public string? ColorTag { get; } = "green";
    }
}
