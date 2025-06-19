using System.ComponentModel.DataAnnotations;

namespace KBMHttpService.DTOs.Organization
{
    public class CreateOrganizationDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(200)]
        public string? Address { get; set; }
    }
}
