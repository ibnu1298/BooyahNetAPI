using BooyahNetAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace BooyahNetAPI.Dtos.User
{
    public class UserDTO : BaseResponse
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;      
    }

    
}
