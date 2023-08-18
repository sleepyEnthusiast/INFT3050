using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag.Models;

[Table("Game_genre")]
public partial class GameGenre
{
    [Key]
    [Column("subGenreID")]
    public int SubGenreId { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }
}
