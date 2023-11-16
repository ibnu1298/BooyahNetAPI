namespace BooyahNetAPI.Dtos.Customer
{
    public class UserUpdateUsernameDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class UserUpdateEmailDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
