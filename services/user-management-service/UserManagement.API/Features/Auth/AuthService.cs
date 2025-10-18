using Microsoft.AspNetCore.Identity;
using UserManagement.Data;

namespace UserManagement.API.Features.Auth
{
    public class AuthService
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        private readonly IJWTGenerator _jwtGenerator;

        public AuthService(UserManager<ApplicationUser> userManager, 
                           SignInManager<ApplicationUser> signInManager,
                           IJWTGenerator jwtGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<bool> RegisterUser(RegisterUserDTO user)
        {
            var newUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                DisplayName = user.DisplayName
            };

            // for now, don't handle errors in detail
            var result = await _userManager.CreateAsync(newUser, user.Password);
            return result.Succeeded;
        }
        
        public async Task<string> LoginUser(LoginUserDTO user)
        {
            string token = string.Empty;
            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.FindByEmailAsync(user.Email);
                token = _jwtGenerator.GenerateToken(appUser);
            }

            return token;

        }
    }
}
