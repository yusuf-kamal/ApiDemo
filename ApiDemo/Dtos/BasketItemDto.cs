using System.ComponentModel.DataAnnotations;

namespace ApiDemo.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]

        public string ProductName { get; set; }
        [Required]
        [Range(1,double.MaxValue,ErrorMessage ="price must be more than zero")]
        public decimal Price { get; set; }
        [Required]

        public int Quntity { get; set; }
        [Required]

        public string PictureUrl { get; set; }
        [Required]

        public string Brand { get; set; }
        [Required]

        public string Type { get; set; }
    }
}