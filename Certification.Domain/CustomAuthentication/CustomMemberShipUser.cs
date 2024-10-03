
using Microsoft.AspNetCore.Identity;
using Certification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;



namespace CertificationWeb.CustomAuthentication
{
    public class CustomMembershipUser : IdentityUser
    {
        #region User Properties

        public int UserId { get; set; }
        public string FullName { get; set; }



        public bool? IsActive { get; set; }
        public bool? IsEmailVerified { get; set; }
        public bool? IsDeleted { get; set; }

        #endregion

        public CustomMembershipUser(User user)
        {
            UserId = user.UserId;
            FullName = user.NameAr;
            UserName = user.NameAr;
            Email = user.Email;

            IsActive = user.IsActive;
            IsDeleted = user.IsDeleted;
            IsEmailVerified = user.IsEmailVerified;

        }
    }
}