using ExpenseApp.Application.Authorization;
using ExpenseApp.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Api.Controllers;

public class AuthController(ISender sender) : BaseApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
    {
        var result = await sender.Send(new LoginUserCommand(loginUserDto));

        if (result.IsFailure)
        {
            return HandleError(result.Error);
        }

        return Ok(new { Token = result.Value.Token, Email = result.Value.Email });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var result = await sender.Send(new RegisterUserCommand(registerUserDto));

        if (result.IsFailure)
        {
            return HandleError(result.Error);
        }

        return Ok(new { Token = result.Value.Token, Email = result.Value.Email });
    }
}
