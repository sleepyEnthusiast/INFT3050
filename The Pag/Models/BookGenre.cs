using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag;

[Table("Book_genre")]
public partial class BookGenre
{
    [Key]
    [Column("subGenreID")]
    public int SubGenreId { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }
}
