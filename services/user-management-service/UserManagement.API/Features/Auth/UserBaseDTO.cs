namespace UserManagement.API.Features.Auth
{
    public abstract class UserBaseDTO
    {
        public string Email { get; set; }   
        public string Password { get; set; }
    }

    public class RegisterUserDTO : UserBaseDTO
    {
        public string DisplayName { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class LoginUserDTO : UserBaseDTO
    {
    }
}
