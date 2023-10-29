using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag;

[Table("Genre")]
public partial class Genre
{
    [Key]
    [Column("genreID")]
    public int GenreId { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    [InverseProperty("GenreNavigation")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("GenreNavigation")]
    public virtual ICollection<Source> Sources { get; set; } = new List<Source>();
}
