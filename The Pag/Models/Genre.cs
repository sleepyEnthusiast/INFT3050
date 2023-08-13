using System.ComponentModel.DataAnnotations;
namespace The_Pag.Models
{
    public class Genre
    {
        [Required]
        public int genreID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
