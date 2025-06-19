using System.ComponentModel.DataAnnotations;

namespace KBMHttpService.DTOs.User
{
    public class AssociateUserDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
    }
}
