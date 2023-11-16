namespace BooyahNetAPI.Dtos.Payment
{
    public class CreatePaymentDTO
    {
        public int CustomerID { get; set; }
        public int PackageID { get; set; }
        public long PricePayment { get; set; }
    }
}
