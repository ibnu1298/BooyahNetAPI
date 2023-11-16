using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooyahNetAPI.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public int MaxUser { get; set; }
        public int MaxBandwidth { get; set; }
        public long PricePackage { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
