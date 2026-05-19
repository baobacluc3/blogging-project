using System.Security.Claims;
using BloggingPlatform.Application.Common;
using BloggingPlatform.Application.DTOs.Common;
using BloggingPlatform.Application.DTOs.Posts;
using BloggingPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.API.Controllers;

[ApiController, Route("api/posts")]
public class PostsController(IPostService postService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] PostQuery query)
    {
        var result = await postService.GetAsync(query);
        return Ok(ApiResponse<PagedResult<PostResponse>>.Ok(result));
    }

    [HttpGet("{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await postService.GetBySlugAsync(slug);
        return Ok(ApiResponse<PostResponse>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "Author,Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest req)
    {
        var userId = GetActorId();
        var created = await postService.CreateAsync(userId, req);
        return Ok(ApiResponse<PostResponse>.Ok(created, "Post created"));
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Author,Admin")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] PostStatusUpdateRequest req)
    {
        var userId = GetActorId();
        var updated = await postService.ChangeStatusAsync(userId, id, req);
        return Ok(ApiResponse<PostResponse>.Ok(updated, "Post status updated"));
    }

    private Guid GetActorId()
    {
        var actorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(actorId) || !Guid.TryParse(actorId, out var userId))
            throw new AppException("Invalid access token", 401);

        return userId;
    }
}
