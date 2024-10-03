using Certification.Domain.Entities;
using Certification.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CertificationWeb.CustomAuthentication
{
    public class CustomMembership : IdentityUser
    {
        private readonly UserManager<CustomMembershipUser> _userManager;
        private readonly SignInManager<CustomMembershipUser> _signInManager;
        private readonly Context _db;

        public CustomMembership(UserManager<CustomMembershipUser> userManager,
            SignInManager<CustomMembershipUser> signInManager, Context db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.IsDeleted == true || user.IsActive != true || user.IsEmailVerified != true)
            {
                return false;
            }


            return await _userManager.CheckPasswordAsync(user, password);
        }


        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            var customUser = new CustomMembershipUser(user);
            return await _userManager.CreateAsync(customUser, password);
        }


        public async Task<CustomMembershipUser> GetUserAsync(string username, bool userIsOnline)
        {
            var user = await _userManager.FindByNameAsync(username);


            if (user == null || user.IsDeleted == true)
            {
                return null;
            }


            return user;
        }


        public async Task<string> GetUserNameByEmailAsync(string email)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted != true);


            return user != null ? user.FullName : string.Empty;
        }


        #region

        public string ApplicationName { get; set; } // Set this as needed

        public bool EnablePasswordReset => true; // Adjust according to your application needs

        public bool EnablePasswordRetrieval => false; // Typically false in modern applications

        public int MaxInvalidPasswordAttempts => 5; // Set as per your security policy

        public int MinRequiredNonAlphanumericCharacters => 1; // Adjust based on your password policy

        public int MinRequiredPasswordLength => 8; // Typically set to a minimum length of 8

        public int PasswordAttemptWindow => 10; // The time frame for counting invalid attempts

        public string PasswordStrengthRegularExpression =>
            "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+]).{8,}$"; // Example regex for strong password

        public bool RequiresQuestionAndAnswer => false; // Typically false in modern applications

        public bool RequiresUniqueEmail => true; // Ensure unique email addresses

        public async Task<bool> ChangePasswordAsync(string username, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<List<CustomMembershipUser>> FindUsersByEmailAsync(string emailToMatch)
        {
            return await _userManager.Users
                .Where(x => x.Email.Contains(emailToMatch))
                .ToListAsync();
        }

        public async Task<List<CustomMembershipUser>> FindUsersByNameAsync(string usernameToMatch)
        {
            return await _userManager.Users
                .Where(x => x.UserName.Contains(usernameToMatch))
                .ToListAsync();
        }

        public async Task<List<CustomMembershipUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<int> GetNumberOfUsersOnlineAsync()
        {
            // Implement your logic for online user tracking if needed
            return 0; // Return the count of online users
        }

        public async Task<string> GetPasswordAsync(string username)
        {
            // Implement password retrieval logic if necessary (usually not recommended)
            return null; // Typically, you won't retrieve passwords
        }

        public async Task<CustomMembershipUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<string> ResetPasswordAsync(string username, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return null;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded ? "Password reset successful" : "Password reset failed";
        }

        public async Task<bool> UnlockUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            user.LockoutEnd = null; // Unlock the user
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task UpdateUserAsync(CustomMembershipUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        #endregion
    }
}