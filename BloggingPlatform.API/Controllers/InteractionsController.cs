using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BloggingPlatform.API.Controllers;
[ApiController,Route("api/interactions")]
public class InteractionsController:ControllerBase{
[HttpGet] public IActionResult Get()=>Ok(new {success=true,message="Interactions endpoint ready",data=Array.Empty<object>(),errors=Array.Empty<string>()});
}
