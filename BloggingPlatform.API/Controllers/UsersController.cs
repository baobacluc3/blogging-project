using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BloggingPlatform.API.Controllers;
[ApiController,Route("api/users")]
public class UsersController:ControllerBase{
[HttpGet] public IActionResult Get()=>Ok(new {success=true,message="Users endpoint ready",data=Array.Empty<object>(),errors=Array.Empty<string>()});
}
