using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using System.Security.Claims;

namespace SPD.MVC.Geral.Hubs
{
    public class SPDUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            if (request.User.Identity.IsAuthenticated == false || String.IsNullOrWhiteSpace(request.User.Identity.Name))
            {
                // Usuario ainda não fez login. Nada a fazer
                return String.Empty;
            }

            var claimsIdentity = request.User.Identity as ClaimsIdentity;
            var sidClaim = claimsIdentity.Claims.Where(claim => claim.Type == ClaimTypes.Sid).FirstOrDefault();

            if (sidClaim == null)
            {
                throw new Exception("The user identity has no Sid claim.");
            }

            return sidClaim.Value;
        }
    }
}
