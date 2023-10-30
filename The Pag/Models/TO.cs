using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag;

[Table("TO")]
[Index("PatronId", Name = "IX_TO_PatronId")]
public partial class To
{
    [Key]
    [Column("customerID")]
    public int CustomerId { get; set; }

    public int? PatronId { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string? PhoneNumber { get; set; }

    [StringLength(255)]
    public string? StreetAddress { get; set; }

    [StringLength(4)]
    public int? PostCode { get; set; }

    [StringLength(50)]
    public string? Suburb { get; set; }

    [StringLength(50)]
    public string? State { get; set; }

    [StringLength(50)]
    public string? CardNumber { get; set; }

    [StringLength(50)]
    public string? CardOwner { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string? Expiry { get; set; }

    [Column("CVV")]
    public int? Cvv { get; set; }

    [InverseProperty("CustomerNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("PatronId")]
    [InverseProperty("Tos")]
    public virtual Patron? Patron { get; set; }
}
