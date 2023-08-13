using System.ComponentModel.DataAnnotations;

namespace The_Pag.Models
{
    public class Product
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Genre { get; set; }
        [Required]
        public int Subgenre { get; set; }
        [Required]
        public DateTime Published { get; set; }
        [Required]
        public string LastUpdatedBy { get; set; }
        [Required]
        public DateTime LastUpdated { get; set; }
    }
}
