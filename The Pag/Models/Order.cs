﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag;

[Index("Customer", Name = "IX_Orders_customer")]
public partial class Order
{
    [Key]
    [Column("OrderID")]
    public int OrderId { get; set; }

    [Column("customer")]
    public int? Customer { get; set; }

    [StringLength(255)]
    public string? StreetAddress { get; set; }

    [StringLength(4)]
    public int? PostCode { get; set; }

    [StringLength(255)]
    public string? Suburb { get; set; }

    [StringLength(50)]
    public string? State { get; set; }

    [ForeignKey("Customer")]
    [InverseProperty("Orders")]
    public virtual To? CustomerNavigation { get; set; }
}
