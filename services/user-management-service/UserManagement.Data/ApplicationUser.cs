using Microsoft.AspNetCore.Identity;

namespace UserManagement.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
