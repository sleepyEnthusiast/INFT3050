using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag.Models;

[Table("Stocktake")]
public partial class Stocktake
{
    [Key]
    public int ItemId { get; set; }

    public int? SourceId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Stocktakes")]
    public virtual Product? Product { get; set; }

    [ForeignKey("SourceId")]
    [InverseProperty("Stocktakes")]
    public virtual Source? Source { get; set; }
}
