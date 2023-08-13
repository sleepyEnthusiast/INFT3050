using System.ComponentModel.DataAnnotations;
namespace The_Pag.Models
{
    public class User
    {
        [Required]
        public int? UserID { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public bool? isAdmin { get; set; }
        [Required]
        public string? Salt { get; set; }
        [Required]
        public string? HashedPW { get; set; } 
    }
}
