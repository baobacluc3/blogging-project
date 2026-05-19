namespace BloggingPlatform.Application.DTOs.Auth;
public record RegisterRequest(string FullName,string Email,string Username,string Password);
public record LoginRequest(string Username,string Password);
public record AuthResponse(string AccessToken,string RefreshToken,DateTime ExpiresAt);
public record RefreshTokenRequest(string RefreshToken);
