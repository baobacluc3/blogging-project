using BloggingPlatform.Application.DTOs.Auth;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace BloggingPlatform.Application.Services;
public class AuthService(AppDbContext db,IJwtTokenService jwt,IPasswordHasher<User> hasher):IAuthService{
public async Task<AuthResponse> RegisterAsync(RegisterRequest req){if(await db.Users.AnyAsync(x=>x.Username==req.Username||x.Email==req.Email)) throw new Exception("User already exists");var u=new User{FullName=req.FullName,Email=req.Email,Username=req.Username};u.PasswordHash=hasher.HashPassword(u,req.Password);db.Users.Add(u);var rf=jwt.GenerateRefreshToken();db.RefreshTokens.Add(new RefreshToken{UserId=u.Id,Token=rf,ExpiresAt=DateTime.UtcNow.AddDays(7)});await db.SaveChangesAsync();return new AuthResponse(jwt.GenerateAccessToken(u.Id,u.Username,u.Role.ToString()),rf,DateTime.UtcNow.AddMinutes(60));}
public async Task<AuthResponse> LoginAsync(LoginRequest req){var u=await db.Users.FirstOrDefaultAsync(x=>x.Username==req.Username&&x.IsActive)??throw new Exception("Invalid credentials");if(hasher.VerifyHashedPassword(u,u.PasswordHash,req.Password)==PasswordVerificationResult.Failed) throw new Exception("Invalid credentials");var rf=jwt.GenerateRefreshToken();db.RefreshTokens.Add(new RefreshToken{UserId=u.Id,Token=rf,ExpiresAt=DateTime.UtcNow.AddDays(7)});await db.SaveChangesAsync();return new AuthResponse(jwt.GenerateAccessToken(u.Id,u.Username,u.Role.ToString()),rf,DateTime.UtcNow.AddMinutes(60));}
public async Task<AuthResponse> RefreshAsync(string token){var rt=await db.RefreshTokens.FirstOrDefaultAsync(x=>x.Token==token&&!x.Revoked&&x.ExpiresAt>DateTime.UtcNow)??throw new Exception("Invalid refresh token");var u=await db.Users.FindAsync(rt.UserId)??throw new Exception("User not found");return new AuthResponse(jwt.GenerateAccessToken(u.Id,u.Username,u.Role.ToString()),token,DateTime.UtcNow.AddMinutes(60));}}
