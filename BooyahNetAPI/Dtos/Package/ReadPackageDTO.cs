namespace BooyahNetAPI.Dtos.Package
{
    public class ReadPackageDTO
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public long PricePackage { get; set; }
        public int MaxUser { get; set; }
        public int MaxBandwidth { get; set; }
    }
}
