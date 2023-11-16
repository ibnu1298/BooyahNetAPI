using BooyahNetAPI.Dtos.Package;

namespace BooyahNetAPI.Dtos.Payment
{
    public class ReadPaymentPackageDTO
    {
        public int Id { get; set; }
        public long PricePayment { get; set; }
        public DateTime Date { get; set; }
        public ReadPackageDTO Package { get; set; }
    }
}
