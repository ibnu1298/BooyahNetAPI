using BooyahNetAPI.Dtos.Payment;

namespace BooyahNetAPI.Dtos.User
{
    public class UserPaymentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public long NumberPhone { get; set; }
        public string Username { get; set; }
        public List<ReadPaymentPackageDTO> Payments { get; set; }
    }
}
