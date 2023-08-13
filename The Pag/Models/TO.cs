using System.ComponentModel.DataAnnotations;
namespace The_Pag.Models
{
    public class TO
    {
        [Required]
        public int? customerID { get; set; }
        [Required]
        public int? PatronId { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? StreetAdress { get; set;}
        [Required]
        public int? PostCode { get; set; }
        [Required]
        public string? Suburb { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? CardNumber { get; set; }
        [Required]
        public string? Expiry { get; set; }
        [Required]
        public int? CVV { get; set; }
    }
}
