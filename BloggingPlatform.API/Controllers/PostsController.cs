using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BloggingPlatform.API.Controllers;
[ApiController,Route("api/posts")]
public class PostsController:ControllerBase{
[HttpGet] public IActionResult Get()=>Ok(new {success=true,message="Posts endpoint ready",data=Array.Empty<object>(),errors=Array.Empty<string>()});
}
