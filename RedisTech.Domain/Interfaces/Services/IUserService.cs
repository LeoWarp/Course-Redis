using RedisTech.Domain.Dto.User;
using RedisTech.Domain.Result;

namespace RedisTech.Domain.Interfaces.Services;

public interface IUserService
{
    Task<BaseResult<bool>> StartTransaction();
    
    Task<BaseResult<UserDto>> AddUserToCache(long userId);
    
    Task<BaseResult<UserDto>> GetUserByKeyAsync(long userId);
    
    Task<BaseResult<UserDto>> RemoveUserByKeyAsync(long userId);
    
    Task<BaseResult<UserDto>> RefreshUserByKeyAsync(long userId);
}