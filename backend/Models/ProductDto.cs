
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Price { get; set; }
        public string Description { get; set; } = string.Empty;
        [Required]
        public IFormFile FileImage { get; set; } 
    }
}