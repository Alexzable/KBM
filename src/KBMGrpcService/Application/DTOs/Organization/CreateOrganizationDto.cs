using System.ComponentModel.DataAnnotations;

namespace KBMGrpcService.Application.DTOs.Organization
{
    public class CreateOrganizationDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        [StringLength(200)]
        public string? Address { get; set; }
    }
}
