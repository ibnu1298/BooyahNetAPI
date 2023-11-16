using AutoMapper;
using BooyahNetAPI.Dtos.Payment;
using BooyahNetAPI.Models;

namespace BooyahNetAPI.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile() 
        { 
            CreateMap<CreatePaymentDTO, Payment>();

            CreateMap<Payment, ReadPaymentDTO>();
            CreateMap<ReadPaymentDTO, Payment>();

            CreateMap<Payment, ReadPaymentPackageDTO>();
            CreateMap<ReadPaymentPackageDTO, Payment>();
        }
    }
}
