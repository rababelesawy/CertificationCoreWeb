using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CertificationWeb.CustomAuthentication
{
    public class CustomPrincipal
    {
        #region Identity Properties

        public int UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserName { get; private set; }

        public int TypeId { get; private set; }
        public int ImageId { get; private set; }
        public int CityId { get; private set; }
        public int CountryId { get; private set; }
        public int CategoryId { get; private set; }

        public string[] Roles { get; private set; }

        #endregion

        public CustomPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException(nameof(claimsPrincipal));
            }

            UserId = Convert.ToInt32(claimsPrincipal.FindFirst("UserId")?.Value);
            FirstName = claimsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value;
            LastName = claimsPrincipal.FindFirst(ClaimTypes.Surname)?.Value;
            UserName = claimsPrincipal.Identity.Name;

            // Example for other properties, adapt according to your claims
            TypeId = Convert.ToInt32(claimsPrincipal.FindFirst("TypeId")?.Value);
            ImageId = Convert.ToInt32(claimsPrincipal.FindFirst("ImageId")?.Value);
            CityId = Convert.ToInt32(claimsPrincipal.FindFirst("CityId")?.Value);
            CountryId = Convert.ToInt32(claimsPrincipal.FindFirst("CountryId")?.Value);
            CategoryId = Convert.ToInt32(claimsPrincipal.FindFirst("CategoryId")?.Value);
            Roles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray();
        }

        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }
    }
}
