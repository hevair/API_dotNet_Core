using Api.Domain.DTOs;
using Api.Domain.Models;
using AutoMapper;

namespace Api.CrossCutting.Mappings
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<UserModel, UserDTO>().ReverseMap();
            CreateMap<UserModel, UserDTOUpdate>().ReverseMap();
            CreateMap<UserModel, UserDTOCreate>().ReverseMap();
        }
    }
}