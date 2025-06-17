using KBMHttpService.Common.Helpers;
using System.ComponentModel.DataAnnotations;

namespace KBMHttpService.API.Features.User.Models.Requests
{
    public class UsersForOrganizationRequest : PaginationParams
    {
        [Required]
        public Guid OrganizationId { get; set; }
    }
}
