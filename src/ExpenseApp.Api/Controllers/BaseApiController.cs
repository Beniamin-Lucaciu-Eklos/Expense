using System;
using ExpenseApp.Api.Constants;
using ExpenseApp.Domain.Abstractions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors(CorsConstants.DefaultPolicy)]
public abstract class BaseApiController : ControllerBase
{
    protected virtual IActionResult HandleError(Error error)
    {
        return error switch
        {
            { Type: ErrorType.NotFound } => NotFound(error.Description),
            { Type: ErrorType.Unauthorized } => Unauthorized(error.Description),
            { Type: ErrorType.Conflict } => Conflict(error.Description),

            _ => BadRequest(error)
        };
    }

}
