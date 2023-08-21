using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag.Models;

public partial class Patron
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(255)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    [StringLength(32)]
    [Unicode(false)]
    public string? Salt { get; set; }

    [Column("HashPW")]
    [StringLength(64)]
    [Unicode(false)]
    public string? HashPw { get; set; }

    [InverseProperty("Patron")]
    public virtual ICollection<To> Tos { get; set; } = new List<To>();
}
