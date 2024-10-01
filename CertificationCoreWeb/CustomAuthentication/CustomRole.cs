using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CertificationCoreWeb.CustomAuthentication
{
    public class CustomRole
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomRole(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Checks if the user is in the specified role.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="roleName">Role to check</param>
        /// <returns>True if the user is in the role, false otherwise</returns>
        public async Task<bool> IsUserInRoleAsync(string username, string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(user, roleName);
        }

        /// <summary>
        /// Gets all roles for a given user.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Array of roles</returns>
        public async Task<string[]> GetRolesForUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Array.Empty<string>();
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles?.ToArray() ?? Array.Empty<string>();
        }

        // You can add more custom role-related logic here if needed



        #region 

  

        public async Task<IdentityResult> AddUsersToRolesAsync(string[] usernames, string[] roleNames)
        {
            IdentityResult result = IdentityResult.Success;

            foreach (var username in usernames)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    foreach (var roleName in roleNames)
                    {
                        if (await _roleManager.RoleExistsAsync(roleName))
                        {
                            result = await _userManager.AddToRoleAsync(user, roleName);
                            if (!result.Succeeded)
                            {
                                return result; // Exit early if any operation fails
                            }
                        }
                    }
                }
            }
            return result;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role already exists." });
            }

            var role = new IdentityRole(roleName);
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName, bool throwOnPopulatedRole)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
            }

            // Check if the role is populated
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            if (usersInRole.Any())
            {
                if (throwOnPopulatedRole)
                {
                    throw new InvalidOperationException("Cannot delete a populated role.");
                }
                return IdentityResult.Failed(new IdentityError { Description = "Role is populated with users." });
            }

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<string[]> FindUsersInRoleAsync(string roleName, string usernameToMatch)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            var matchingUsers = usersInRole.Where(u => u.UserName.Contains(usernameToMatch)).Select(u => u.UserName).ToArray();
            return matchingUsers;
        }

        public async Task<string[]> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(r => r.Name).ToArray();
        }

        public async Task<string[]> GetUsersInRoleAsync(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.Select(u => u.UserName).ToArray();
        }

        public async Task RemoveUsersFromRolesAsync(string[] usernames, string[] roleNames)
        {
            foreach (var username in usernames)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    foreach (var roleName in roleNames)
                    {
                        // Remove user from role
                        await _userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }
            }
        }
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        #endregion



    }



}

