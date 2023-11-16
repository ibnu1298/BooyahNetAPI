using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooyahNetAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PackageID { get; set; }
        public Package Package { get; set; }
        public long PricePayment { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
