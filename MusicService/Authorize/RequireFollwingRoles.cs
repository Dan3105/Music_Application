using Microsoft.AspNetCore.Authorization;

namespace MusicService.Authorize
{
    public class RequireFollwingRoles : IAuthorizationRequirement
    {
        public string[] requiredRoles { get; }
        public RequireFollwingRoles(params string[] requiredRoles)
        {
            this.requiredRoles = requiredRoles;
        }
    }
}
