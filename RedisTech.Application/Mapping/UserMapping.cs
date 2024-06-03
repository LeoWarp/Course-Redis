using AutoMapper;
using RedisTech.Domain.Dto.User;
using RedisTech.Domain.Entity;

namespace RedisTech.Application.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}