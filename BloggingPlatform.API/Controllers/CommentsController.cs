using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BloggingPlatform.API.Controllers;
[ApiController,Route("api/comments")]
public class CommentsController:ControllerBase{
[HttpGet] public IActionResult Get()=>Ok(new {success=true,message="Comments endpoint ready",data=Array.Empty<object>(),errors=Array.Empty<string>()});
}
