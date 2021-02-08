using System.Linq;
using System.Security.Claims;
using LanguageExt;

namespace DutyCycle.API.Authentication
{
    public static class AuthenticationExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            return int.Parse(GetClaim(principal, Constants.IdClaimType).Value);
        }

        public static Option<int> TryUserGetId(this ClaimsPrincipal principal)
        {
            var claimOption = TryGetClaim(principal, Constants.IdClaimType);
            return claimOption.Map(claim => int.Parse(claim.Value));
        }

        public static int GetOrganizationId(this ClaimsPrincipal principal)
        {
            return int.Parse(GetClaim(principal, Constants.OrganizationIdClaimType).Value);
        }

        private static Option<Claim> TryGetClaim(ClaimsPrincipal principal, string type)
        {
            return principal.Claims.SingleOrDefault(claim => claim.Type == type);
        }
        
        private static Claim GetClaim(ClaimsPrincipal principal, string type)
        {
            return principal.Claims.Single(claim => claim.Type == type);
        }
    }
}