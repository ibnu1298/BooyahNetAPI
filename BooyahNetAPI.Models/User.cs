
using Microsoft.AspNetCore.Identity;

namespace BooyahNetAPI.Models
{
    public enum Gender
    {
        Male, Female
    }
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
    public class UserRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual CustomRole Role { get; set; }
    }

    public class CustomRole : IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}