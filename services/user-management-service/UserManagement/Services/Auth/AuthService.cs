using Microsoft.AspNetCore.Identity;
using UserManagement.Data;

namespace UserManagement.API.Services.Auth
{
    public class AuthService
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> RegisterUser(RegisterUserDTO user)
        {
            var newUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                DisplayName = user.DisplayName
            };
            var result = await _userManager.CreateAsync(newUser, user.Password);
            return result.Succeeded;
        }
        
        public async Task<bool> LoginUser(LoginUserDTO user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
            return result.Succeeded;
        }
    }
}
