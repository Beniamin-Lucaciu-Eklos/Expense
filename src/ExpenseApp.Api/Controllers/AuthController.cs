using System;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Application.Dtos;
using ExpenseApp.Domain.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Api.Controllers;

public class AuthController(ISender sender) : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var result = await sender.Send(new RegisterUserCommand(registerUserDto));

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { Token = result.Value.UserToken });
    }

    /*[HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto registerUserDto)
    {
        var result = await sender.Send(new RegisterUserCommand(registerUserDto));

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { Token = result.Value.UserToken});
    }*/
}
