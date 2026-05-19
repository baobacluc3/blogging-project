using BloggingPlatform.Application.DTOs.Auth;
using BloggingPlatform.Application.DTOs.Common;
using BloggingPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace BloggingPlatform.API.Controllers;
[ApiController,Route("api/auth")]
public class AuthController(IAuthService service):ControllerBase{
[HttpPost("register")] public async Task<IActionResult> Register(RegisterRequest req)=>Ok(ApiResponse<AuthResponse>.Ok(await service.RegisterAsync(req),"Register successful"));
[HttpPost("login")] public async Task<IActionResult> Login(LoginRequest req)=>Ok(ApiResponse<AuthResponse>.Ok(await service.LoginAsync(req),"Login successful"));
[HttpPost("refresh-token")] public async Task<IActionResult> Refresh(RefreshTokenRequest req)=>Ok(ApiResponse<AuthResponse>.Ok(await service.RefreshAsync(req.RefreshToken),"Refresh successful"));}
