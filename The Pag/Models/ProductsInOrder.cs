using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag.Models;

[Keyless]
public partial class ProductsInOrder
{
    public int? OrderId { get; set; }

    [Column("produktId")]
    public int? ProduktId { get; set; }

    public int? Quantity { get; set; }

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }

    [ForeignKey("ProduktId")]
    public virtual Stocktake? Produkt { get; set; }
}
