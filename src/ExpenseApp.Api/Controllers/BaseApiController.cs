using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
public abstract class BaseApiController : ControllerBase
{

}
