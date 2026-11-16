using System;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Application.Dtos;
using ExpenseApp.Domain.Data.Models;
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
            return result.Error.Code switch
            {
                UserErrors.Codes.NotFound => NotFound(result.Error.Name),

                UserErrors.Codes.EmailAlreadyExists => Conflict(result.Error.Name),

                UserErrors.Codes.InvalidCredentials => Unauthorized(),

                _ => BadRequest(result.Error)
            };
        }

        return Ok(new { Token = result.Value.Token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var result = await sender.Send(new RegisterUserCommand(registerUserDto));

        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                UserErrors.Codes.NotFound => NotFound(result.Error.Name),

                UserErrors.Codes.EmailAlreadyExists => Conflict(result.Error.Name),

                UserErrors.Codes.InvalidCredentials => Unauthorized(),

                _ => BadRequest(result.Error)
            };
        }

        return Ok(new { Token = result.Value.Token });
    }
}
