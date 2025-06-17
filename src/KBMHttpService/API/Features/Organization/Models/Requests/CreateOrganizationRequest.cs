using System.ComponentModel.DataAnnotations;

namespace KBMHttpService.API.Features.Organization.Models.Requests
{
    public class CreateOrganizationRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(200)]
        public string? Address { get; set; }
    }
}
