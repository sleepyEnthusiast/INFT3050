﻿using System.ComponentModel.DataAnnotations;
namespace The_Pag.Models
{
    public class Book_genre
    {
        [Required]
        public int? subGenreID { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
