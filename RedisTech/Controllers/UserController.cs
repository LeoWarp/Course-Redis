using Microsoft.AspNetCore.Mvc;
using RedisTech.Domain.Dto.User;
using RedisTech.Domain.Interfaces.Services;
using RedisTech.Domain.Result;

namespace RedisTech.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<UserDto>>> AddUserToCache(long id)
    {
        var response = await _userService.AddUserToCache(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet("user-by-key")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<UserDto>>> GetUserByKey(long id)
    {
        var response = await _userService.GetUserByKeyAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpDelete()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<UserDto>>> RemoveUserByKey(long id)
    {
        var response = await _userService.RemoveUserByKeyAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost("start-transaction")]
    public async Task<ActionResult<bool>> StartTransaction()
    {
        var response = await _userService.StartTransaction();
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}