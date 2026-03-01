using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBconnector.Models;

public partial class Grid
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GridId { get; set; }

    public int? LocationId { get; set; }

    public string? Name { get; set; }

    public int Xmax { get; set; }

    public int Ymax { get; set; }

    public virtual ICollection<GridPo> GridPos { get; set; } = new List<GridPo>();

    public virtual Location? Location { get; set; }
}
