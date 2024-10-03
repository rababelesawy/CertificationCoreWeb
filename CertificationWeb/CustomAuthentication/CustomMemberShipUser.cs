
using Microsoft.AspNetCore.Identity;
using Certification.Domain.Entities;


namespace CertificationWeb.CustomAuthentication
{
    public class CustomMembershipUser : IdentityUser
    {
        #region User Properties

        public string UserId { get; set; }
        public string FullName { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsEmailVerified { get; set; }
        public bool? IsDeleted { get; set; }

        #endregion

        public CustomMembershipUser(User user)
        {
            UserId = user.Id;
            FullName = user.NameAr;
            UserName = user.NameAr;
            Email = user.Email;

            IsActive = user.IsActive;
            IsDeleted = user.IsDeleted;
            IsEmailVerified = user.EmailConfirmed;

        }
    }
}