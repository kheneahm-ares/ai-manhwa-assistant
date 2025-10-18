using Microsoft.AspNetCore.Identity;
using UserManagement.Data;

namespace UserManagement.API.Features.User
{
    public class UsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        // update display name
        public async Task<bool> UpdateDisplayNameAsync(string email, string newDisplayName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;
            user.DisplayName = newDisplayName;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
