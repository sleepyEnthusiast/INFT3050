using System.ComponentModel.DataAnnotations;
namespace The_Pag.Models
{
    public class Stocktake
    {
        [Required]
        public int? ItemId { get; set; }
        [Required]
        public int? SourceId { get; set;}
        [Required]
        public int? ProductId { get; set;}
        [Required]
        public int? Quantity { get; set;}
        [Required]
        public float? Price { get; set;}
    }
}
