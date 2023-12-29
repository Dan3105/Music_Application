using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MusicService.Authorize
{
    public class RequireFollowingRolesHandler : AuthorizationHandler<RequireFollwingRoles>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireFollwingRoles requirement)
        {
            try
            {
                var userRoles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
                var intersectedRoles = userRoles.Intersect(requirement.requiredRoles);

                if (intersectedRoles.Any())
                {
                    context.Succeed(requirement);
                }
                return Task.CompletedTask;
            }
            catch (Exception e){ 
                Console.WriteLine(e.ToString());
                return Task.CompletedTask;

            }
        }
    }
}
