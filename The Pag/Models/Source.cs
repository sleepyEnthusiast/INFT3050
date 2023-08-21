using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag.Models;

[Table("Source")]
public partial class Source
{
    [Key]
    [Column("sourceid")]
    public int Sourceid { get; set; }

    [Column("Source_name")]
    public string? SourceName { get; set; }

    [Column("externalLink")]
    public string? ExternalLink { get; set; }

    public int? Genre { get; set; }

    [ForeignKey("Genre")]
    [InverseProperty("Sources")]
    public virtual Genre? GenreNavigation { get; set; }

    [InverseProperty("Source")]
    public virtual ICollection<Stocktake> Stocktakes { get; set; } = new List<Stocktake>();
}
