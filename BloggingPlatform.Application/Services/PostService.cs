using BloggingPlatform.Application.Common;
using BloggingPlatform.Application.DTOs.Common;
using BloggingPlatform.Application.DTOs.Posts;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.Domain.Enums;
using BloggingPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatform.Application.Services;

public class PostService(AppDbContext db) : IPostService
{
    public async Task<PostResponse> CreateAsync(Guid actorId, CreatePostRequest req)
    {
        ValidatePostRequest(req);

        var author = await db.Users.FirstOrDefaultAsync(x => x.Id == actorId && x.IsActive)
            ?? throw new AppException("Author not found", 404);

        var category = await db.Categories.FirstOrDefaultAsync(x => x.Id == req.CategoryId)
            ?? throw new AppException("Category not found", 404);

        var slug = await CreateUniqueSlugAsync(req.Title);

        var post = new Post
        {
            AuthorId = actorId,
            CategoryId = req.CategoryId,
            Title = req.Title.Trim(),
            Summary = req.Summary?.Trim(),
            Content = req.Content.Trim(),
            Slug = slug,
            Status = PostStatus.Draft
        };

        if (req.TagIds.Count > 0)
        {
            var existingTagIds = await db.Tags.Where(x => req.TagIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();
            post.PostTags = existingTagIds.Distinct().Select(tagId => new PostTag { TagId = tagId, PostId = post.Id }).ToList();
        }

        db.Posts.Add(post);
        await db.SaveChangesAsync();
        return MapResponse(post, author.FullName, category.Name);
    }

    public async Task<PagedResult<PostResponse>> GetAsync(PostQuery query)
    {
        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize is <= 0 or > 50 ? 10 : query.PageSize;

        var posts = db.Posts.AsNoTracking().Include(x => x.Author).Include(x => x.Category).Include(x => x.PostTags).ThenInclude(x => x.Tag).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            posts = posts.Where(x => x.Title.Contains(search) || (x.Summary != null && x.Summary.Contains(search)));
        }

        if (query.CategoryId.HasValue) posts = posts.Where(x => x.CategoryId == query.CategoryId.Value);
        if (query.AuthorId.HasValue) posts = posts.Where(x => x.AuthorId == query.AuthorId.Value);
        if (query.TagId.HasValue) posts = posts.Where(x => x.PostTags.Any(t => t.TagId == query.TagId.Value));
        if (query.Status.HasValue) posts = posts.Where(x => x.Status == query.Status.Value);

        posts = query.SortBy.ToLowerInvariant() switch
        {
            "title" => query.Desc ? posts.OrderByDescending(x => x.Title) : posts.OrderBy(x => x.Title),
            "createdat" => query.Desc ? posts.OrderByDescending(x => x.CreatedAt) : posts.OrderBy(x => x.CreatedAt),
            _ => query.Desc ? posts.OrderByDescending(x => x.PublishedAt).ThenByDescending(x => x.CreatedAt) : posts.OrderBy(x => x.PublishedAt).ThenBy(x => x.CreatedAt)
        };

        var total = await posts.LongCountAsync();
        var items = await posts.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new PostResponse(
            x.Id, x.Title, x.Slug, x.Summary ?? string.Empty, x.Content, x.AuthorId,
            x.Author != null ? x.Author.FullName : string.Empty,
            x.CategoryId, x.Category != null ? x.Category.Name : string.Empty,
            x.Status, x.ViewCount, x.PublishedAt,
            x.PostTags.Select(pt => pt.Tag != null ? pt.Tag.Name : string.Empty))).ToListAsync();

        return new PagedResult<PostResponse>(items, page, pageSize, total);
    }

    public async Task<PostResponse> GetBySlugAsync(string slug, bool includeUnpublished = false)
    {
        var normalized = slug.Trim().ToLowerInvariant();
        var post = await db.Posts.Include(x => x.Author).Include(x => x.Category).Include(x => x.PostTags).ThenInclude(x => x.Tag)
            .FirstOrDefaultAsync(x => x.Slug.ToLower() == normalized)
            ?? throw new AppException("Post not found", 404);

        if (!includeUnpublished && post.Status != PostStatus.Published)
            throw new AppException("Post is not published", 403);

        post.ViewCount += 1;
        await db.SaveChangesAsync();
        return MapResponse(post, post.Author?.FullName ?? string.Empty, post.Category?.Name ?? string.Empty);
    }

    public async Task<PostResponse> ChangeStatusAsync(Guid actorId, Guid postId, PostStatusUpdateRequest req)
    {
        var actor = await db.Users.FirstOrDefaultAsync(x => x.Id == actorId && x.IsActive) ?? throw new AppException("User not found", 404);
        var post = await db.Posts.Include(x => x.Author).Include(x => x.Category).Include(x => x.PostTags).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.Id == postId)
            ?? throw new AppException("Post not found", 404);

        var isOwner = post.AuthorId == actorId;
        var isAdmin = actor.Role == UserRole.Admin;
        if (!isOwner && !isAdmin) throw new AppException("You are not allowed to update this post", 403);

        post.Status = req.Status;
        post.PublishedAt = req.Status == PostStatus.Published ? DateTime.UtcNow : null;
        await db.SaveChangesAsync();

        return MapResponse(post, post.Author?.FullName ?? string.Empty, post.Category?.Name ?? string.Empty);
    }

    private static void ValidatePostRequest(CreatePostRequest req)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(req.Title)) errors.Add("Title is required");
        if (string.IsNullOrWhiteSpace(req.Content)) errors.Add("Content is required");
        if (req.CategoryId == Guid.Empty) errors.Add("CategoryId is required");

        if (errors.Count > 0) throw new AppException("Validation failed", 400, errors);
    }

    private static PostResponse MapResponse(Post post, string authorName, string categoryName) =>
        new(post.Id, post.Title, post.Slug, post.Summary ?? string.Empty, post.Content, post.AuthorId, authorName, post.CategoryId, categoryName, post.Status, post.ViewCount, post.PublishedAt, post.PostTags.Select(x => x.Tag?.Name ?? x.TagId.ToString()));

    private async Task<string> CreateUniqueSlugAsync(string title)
    {
        var baseSlug = string.Join('-', title.Trim().ToLowerInvariant().Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries));
        var slug = baseSlug;
        var index = 1;

        while (await db.Posts.AnyAsync(x => x.Slug == slug)) slug = $"{baseSlug}-{index++}";
        return slug;
    }
}
