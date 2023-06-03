using System.ComponentModel.DataAnnotations;

namespace ApiDemo.Dtos
{
    public class ReqisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
    }
}
