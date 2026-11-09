using System;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{

}
