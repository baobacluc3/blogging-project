using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BloggingPlatform.API.Controllers;
[ApiController,Route("api/categories")]
public class CategoriesController:ControllerBase{
[HttpGet] public IActionResult Get()=>Ok(new {success=true,message="Categories endpoint ready",data=Array.Empty<object>(),errors=Array.Empty<string>()});
}
