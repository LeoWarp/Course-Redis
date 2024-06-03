using System.Diagnostics;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RedisTech.Domain.Dto.User;
using RedisTech.Domain.Entity;
using RedisTech.Domain.Extensions;
using RedisTech.Domain.Interfaces.Repositories;
using RedisTech.Domain.Interfaces.Services;
using RedisTech.Domain.Result;
using StackExchange.Redis;

namespace RedisTech.Application.Services;

public class UserService : IUserService
{
    private readonly IDatabase _database; 
    private readonly IBaseRepository<User> _userRepository;
    private readonly IDistributedCache _distributedCache;
    private readonly IMapper _mapper;

    public UserService(IBaseRepository<User> userRepository, IDistributedCache distributedCache, IMapper mapper, IDatabase database)
    {
        _userRepository = userRepository;
        _distributedCache = distributedCache;
        _mapper = mapper;
        _database = database;
    }

    public async Task<BaseResult<bool>> StartTransaction()
    {
        var trans = _database.CreateTransaction();

        List<Task> tasks = new List<Task>();

        for (int i = 0; i < 5; i++)
        {
            tasks.Add(trans.StringSetAsync($"Key_{i}", $"Value_{i}"));
        }

        trans.AddCondition(Condition.KeyExists("Key_1"));
        
        bool isCommitted = await trans.ExecuteAsync();
        if (isCommitted)
        {
            try
            {
                await Task.WhenAll(tasks);
            
                return new BaseResult<bool>()
                {
                    Data = true
                };
            }
            catch (Exception ex)
            {
                // 
            }
        }

        return new BaseResult<bool>()
        {
            Data = false
        };
    }
    
    public async Task<BaseResult<UserDto>> AddUserToCache(long userId)
    {
        var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            return new BaseResult<UserDto>()
            {
                ErrorMessage = "Пользователь не найден"
            };
        }
        
        var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

        #region MemoryCache

        //var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
        // _memoryCache.Set($"User_{user.Id}", user, options);

        #endregion
        
        _distributedCache.SetObject($"User_{user.Id}", user, options);
        Debug.Write($"В кеш добавился ключ User_{user.Id}");

        return new BaseResult<UserDto>()
        {
            Data = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<BaseResult<UserDto>> GetUserByKeyAsync(long userId)
    {
        #region MemoryCache
        
        // var originalUser = _memoryCache.Get<User>($"User_{userId}");
        
        #endregion
        
        var originalUser = _distributedCache.GetObject<User>($"User_{userId}");
        if (originalUser == null)
        {
            Debug.WriteLine("Пользователь не найден в хранилище, попробуем вытянуть из БД");
        }

        originalUser = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == userId);
        if (originalUser == null)
        {
            Debug.WriteLine("Пользователь не найден в БД");

            return new BaseResult<UserDto>();
        }
        
        return new BaseResult<UserDto>()
        {
            Data = _mapper.Map<UserDto>(originalUser)
        };
    }

    public async Task<BaseResult<UserDto>> RemoveUserByKeyAsync(long userId)
    {
        #region Проверка

        var originalUser = _distributedCache.GetObject<User>($"User_{userId}");
        if (originalUser != null)
        {
            _distributedCache.Remove($"User_{userId}");
            Debug.WriteLine($"Пользователь c идентификатором {userId} удален из кеша");
        }

        #endregion
        
        await _distributedCache.RemoveAsync($"User_{userId}");

        return new BaseResult<UserDto>();
    }

    public async Task<BaseResult<UserDto>> RefreshUserByKeyAsync(long userId)
    {
        await _distributedCache.RefreshAsync($"User_{userId}");

        _distributedCache.RefreshObject<User>($"User_{userId}");
        
        return new BaseResult<UserDto>();
    }
}