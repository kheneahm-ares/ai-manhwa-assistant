namespace UserManagement.API.Features.Auth
{
    public abstract class UserBase
    {
        public string Email { get; set; }   
        public string Password { get; set; }
    }

    public class RegisterUserDTO : UserBase
    {
        public string DisplayName { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class LoginUserDTO : UserBase
    {
    }
}
