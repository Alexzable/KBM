using KBMHttpService.Shared.Helpers;
using System.ComponentModel.DataAnnotations;

namespace KBMHttpService.DTOs.User
{
    public class UsersForOrganizationDto : PaginationParams
    {
        [Required]
        public Guid OrganizationId { get; set; }
    }
}
