using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag;

[Table("Stocktake")]
[Index("ProductId", Name = "IX_Stocktake_ProductId")]
[Index("SourceId", Name = "IX_Stocktake_SourceId")]
public partial class Stocktake
{
    [Key]
    public int ItemId { get; set; }

    public int? SourceId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    [Required]
    public double? Price { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Stocktakes")]
    public virtual Product? Product { get; set; }

    [ForeignKey("SourceId")]
    [InverseProperty("Stocktakes")]
    public virtual Source? Source { get; set; }
}
