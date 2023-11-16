
using BooyahNetAPI.Dtos;
using BooyahNetAPI.Models;
using System.Globalization;

namespace BooyahNetAPI.Authentication
{
        public class AuthenticateResponse : BaseResponse
        {
            public string UserId { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public Gender Gender { get; set; }
            public string PhoneNumber { get; set; }
            public string Username { get; set; }
            public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            UserId = user.Id;
            Firstname = ti.ToTitleCase(user.FirstName.ToLower());
            Lastname = user.LastName;
            Gender = user.Gender;
            Address = user.Address;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Username = user.UserName;
            if (token != null) { Token = token; }
            
        }
    }
   
}
