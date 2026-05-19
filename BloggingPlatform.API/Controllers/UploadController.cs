using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BloggingPlatform.API.Controllers;
[ApiController,Route("api/upload")]
public class UploadController:ControllerBase{
[HttpGet] public IActionResult Get()=>Ok(new {success=true,message="Upload endpoint ready",data=Array.Empty<object>(),errors=Array.Empty<string>()});
}
