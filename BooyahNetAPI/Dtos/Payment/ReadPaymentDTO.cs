using BooyahNetAPI.Dtos.Package;

namespace BooyahNetAPI.Dtos.Payment
{
    public class ReadPaymentDTO
    {
        public int Id { get; set; }
        public long PricePayment { get; set; }
        public DateTime Date { get; set; }
    }
}
