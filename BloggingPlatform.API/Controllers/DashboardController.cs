using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BloggingPlatform.API.Controllers;
[ApiController,Route("api/dashboard")]
public class DashboardController:ControllerBase{
[HttpGet] public IActionResult Get()=>Ok(new {success=true,message="Dashboard endpoint ready",data=Array.Empty<object>(),errors=Array.Empty<string>()});
}
