using System.ComponentModel.DataAnnotations;

namespace UserService.Model
{
    public class RegisterDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public RegisterDTO() { }
    }
}
