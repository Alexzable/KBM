using System.ComponentModel.DataAnnotations;

namespace KBMHttpService.API.Features.User.Models.Requests
{
    public class AssociateUserRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
    }
}
