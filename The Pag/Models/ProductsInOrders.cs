using System.ComponentModel.DataAnnotations;
namespace The_Pag.Models
{
    public class ProductsInOrders
    {
        [Required]
        public int? OrderId { get; set; }
        [Required]
        public int? ProduktId { get; set; }
        [Required]
        public int? Quantity { get; set; }
    }
}
