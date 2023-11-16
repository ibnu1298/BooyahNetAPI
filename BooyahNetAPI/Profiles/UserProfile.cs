using AutoMapper;
using BooyahNetAPI.Authentication;
using BooyahNetAPI.Dtos.User;
using BooyahNetAPI.Models;

namespace BooyahNetAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<CreateUserDTO, UserDTO>();

            CreateMap<CreateUserDTO, User>();
            CreateMap<User, CreateUserDTO>();

            CreateMap<UserPaymentDTO, User>();
            CreateMap<User, UserPaymentDTO>();
        }
    }
}
