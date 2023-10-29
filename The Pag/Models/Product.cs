using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag;

[Table("Product")]
[Index("Genre", Name = "IX_Product_Genre")]
[Index("LastUpdatedBy", Name = "IX_Product_LastUpdatedBy")]
public partial class Product
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    [StringLength(255)]
    public string? Author { get; set; }

    public string? Description { get; set; }

    public int? Genre { get; set; }

    [Column("subGenre")]
    public int? SubGenre { get; set; }

    [Column(TypeName = "date")]
    public DateTime? Published { get; set; }

    [StringLength(50)]
    public string? LastUpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdated { get; set; }

    [ForeignKey("Genre")]
    [InverseProperty("Products")]
    public virtual Genre? GenreNavigation { get; set; }

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("Products")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Stocktake> Stocktakes { get; set; } = new List<Stocktake>();
}
