using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag.Models;

[Table("User")]
public partial class User
{
    [Column("UserId")]
    public int UserId { get; set; }

    [Key]
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(255)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    [Column("isAdmin")]
    public bool? IsAdmin { get; set; }

    [StringLength(32)]
    [Unicode(false)]
    public string? Salt { get; set; }

    [Column("HashPW")]
    [StringLength(64)]
    [Unicode(false)]
    public string? HashPw { get; set; }

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
