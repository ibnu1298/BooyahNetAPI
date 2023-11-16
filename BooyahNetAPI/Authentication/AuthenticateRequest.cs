using System.ComponentModel.DataAnnotations;

namespace BooyahNetAPI.Authentication
{
    public class AuthenticateRequest
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
