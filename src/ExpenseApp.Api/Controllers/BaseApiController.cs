using System;
using ExpenseApp.Api.Constants;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors(CorsConstants.DefaultPolicy)]
public abstract class BaseApiController : ControllerBase
{

}
