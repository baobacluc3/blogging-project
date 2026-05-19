using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BloggingPlatform.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace BloggingPlatform.Infrastructure.Services;
public class JwtTokenService(IConfiguration cfg):IJwtTokenService{
public string GenerateAccessToken(Guid id,string username,string role){var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Secret"]!));var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);var token=new JwtSecurityToken(claims:[new Claim(ClaimTypes.NameIdentifier,id.ToString()),new Claim(ClaimTypes.Name,username),new Claim(ClaimTypes.Role,role)],expires:DateTime.UtcNow.AddMinutes(60),signingCredentials:creds);return new JwtSecurityTokenHandler().WriteToken(token);} 
public string GenerateRefreshToken()=>Convert.ToBase64String(Guid.NewGuid().ToByteArray())+Convert.ToBase64String(Guid.NewGuid().ToByteArray());}
