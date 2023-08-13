using System.ComponentModel.DataAnnotations;
namespace The_Pag.Models
{
    public class Patrons
    {
        [Required]
        public int? UserId { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Salt { get; set; }
        [Required]
        public string? HashedPW { get; set; }
    }
}
