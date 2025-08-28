using System.ComponentModel.DataAnnotations;

namespace MCPPoC.Server
{
    public class Animal
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0.")]
        public int Id { get; set; }

        [Required]
        public AnimalType Type { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name must be 2-30 characters.")]
        [RegularExpression(@"^(?! )[A-Za-z ]{2,30}(?<! )$", ErrorMessage = "Name must contain only letters and spaces, no leading/trailing/consecutive spaces.")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 40, ErrorMessage = "Age must be between 0 and 40.")]
        public int Age { get; set; }
    }
}
