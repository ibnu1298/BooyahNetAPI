using BooyahNetAPI.Models;

namespace BooyahNetAPI.Dtos.Payment
{
    public class ReadPaymentCustomerDTO
    {
        public int Id { get; set; }
        public ReadDTO CustomerName { get; set; }
        public long PricePayment { get; set; }
        public DateTime Date { get; set; }
    }
}
