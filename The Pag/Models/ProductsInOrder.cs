using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag;

[Keyless]
[Index("OrderId", Name = "IX_ProductsInOrders_OrderId")]
[Index("ProduktId", Name = "IX_ProductsInOrders_produktId")]
public partial class ProductsInOrder
{
    public int? OrderId { get; set; }

    [Column("produktId")]
    public int? ProduktId { get; set; }

    [Required]
    public int? Quantity { get; set; }

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }

    [ForeignKey("ProduktId")]
    public virtual Stocktake? Produkt { get; set; }
}
