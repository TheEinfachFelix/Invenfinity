using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs
{
    public class DTOPart : IDotPart
    {
        internal DTOPart(int PartId)
        {
            this.PartId = PartId;
        }

        public int PartId { get; }
        public string PartName { get; } = "Bauteil";



        // Label / PDF Bezug
        //public string? LabelTemplateId { get; }   // z.B. "gridfinity-part-small"
        //public string? GeneratedLabelPdfPath { get; } // optional Pfad/URI zur generierten PDF

        // Darstellungshilfen
        //public string? SmallIconUri { get; }      // Thumbnail / Icon für die UI
        public string? ColorTag { get; } = "green";
    }
}
