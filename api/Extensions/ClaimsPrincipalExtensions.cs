using System.Security.Claims;

namespace api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return  user.FindFirst(ClaimTypes.Name)?.Value;
        }

         public static int GetUserId(this ClaimsPrincipal user)
        {
            return  Int32.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}