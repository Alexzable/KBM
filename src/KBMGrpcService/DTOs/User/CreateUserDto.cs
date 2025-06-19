using System.ComponentModel.DataAnnotations;

namespace KBMGrpcService.Application.DTOs.User
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;
    }
}
