using System.ComponentModel.DataAnnotations;
namespace The_Pag.Models
{
    public class Source
    {
        [Required]
        public int? sourceid { get; set; }
        [Required]
        public string? Source_name { get; set; }
        [Required]
        public string? externalLink { get; set; }
        [Required]
        public int? Genre { get; set; }
    }
}
