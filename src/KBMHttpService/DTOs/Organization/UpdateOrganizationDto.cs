using System.ComponentModel.DataAnnotations;

namespace KBMHttpService.DTOs.Organization
{
    public class UpdateOrganizationDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(200)]
        public string? Address { get; set; }
    }
}
