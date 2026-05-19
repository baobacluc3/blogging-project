using BloggingPlatform.Application.DTOs.Auth;
using BloggingPlatform.Application.DTOs.Posts;
using BloggingPlatform.Application.DTOs.Common;
namespace BloggingPlatform.Application.Interfaces;
public interface IAuthService{Task<AuthResponse> RegisterAsync(RegisterRequest req);Task<AuthResponse> LoginAsync(LoginRequest req);Task<AuthResponse> RefreshAsync(string token);} 
public interface IPostService{Task<PostResponse> CreateAsync(Guid actorId,CreatePostRequest req);Task<PagedResult<PostResponse>> GetAsync(PostQuery query);} 
public interface IJwtTokenService{string GenerateAccessToken(Guid id,string username,string role);string GenerateRefreshToken();}
