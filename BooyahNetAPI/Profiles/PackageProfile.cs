using AutoMapper;
using BooyahNetAPI.Dtos.Package;
using BooyahNetAPI.Dtos.Payment;
using BooyahNetAPI.Models;

namespace BooyahNetAPI.Profiles
{
    public class PackageProfile : Profile
    {
        public PackageProfile() 
        { 
            CreateMap<CreatePackageDTO, Package>();

            CreateMap<Package, ReadPackageDTO>();
            CreateMap<ReadPackageDTO, Package>();
        }
    }
}
