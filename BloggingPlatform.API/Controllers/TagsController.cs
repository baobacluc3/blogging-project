using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BloggingPlatform.API.Controllers;
[ApiController,Route("api/tags")]
public class TagsController:ControllerBase{
[HttpGet] public IActionResult Get()=>Ok(new {success=true,message="Tags endpoint ready",data=Array.Empty<object>(),errors=Array.Empty<string>()});
}
