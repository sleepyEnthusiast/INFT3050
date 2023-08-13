using System.ComponentModel.DataAnnotations;
namespace The_Pag.Models
{
    public class Orders
    {
        [Required]
        public int? OrderID { get; set; }
        [Required]
        public int? customer { get; set; }
        [Required]
        public string? StreetAddress { get; set; }
        [Required]
        public int? PostCode { get; set; }
        [Required]
        public string? Suburb { get; set; }
        [Required]
        public string? State { get; set; }
    }
}
