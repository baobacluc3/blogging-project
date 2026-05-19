using BloggingPlatform.Domain.Enums;
namespace BloggingPlatform.Application.DTOs.Posts;
public record CreatePostRequest(string Title,string Summary,string Content,Guid CategoryId,List<Guid> TagIds);
public record UpdatePostRequest(string Title,string Summary,string Content,Guid CategoryId,List<Guid> TagIds);
public record PostQuery(string? Search,Guid? CategoryId,Guid? AuthorId,Guid? TagId,PostStatus? Status,int Page=1,int PageSize=10,string SortBy="publishedAt",bool Desc=true);
public record PostResponse(Guid Id,string Title,string Slug,string Summary,string Content,Guid AuthorId,string AuthorName,Guid CategoryId,string CategoryName,PostStatus Status,int ViewCount,DateTime? PublishedAt,IEnumerable<string> Tags);
