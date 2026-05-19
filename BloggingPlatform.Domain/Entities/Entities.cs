using BloggingPlatform.Domain.Common;
using BloggingPlatform.Domain.Enums;
namespace BloggingPlatform.Domain.Entities;
public class User:BaseEntity{public string FullName{get;set;}="";public string Email{get;set;}="";public string Username{get;set;}="";public string PasswordHash{get;set;}="";public string? AvatarUrl{get;set;}public string? Bio{get;set;}public UserRole Role{get;set;}=UserRole.User;public bool IsActive{get;set;}=true;public ICollection<Post> Posts{get;set;}=new List<Post>();}
public class Category:BaseEntity{public string Name{get;set;}="";public string Slug{get;set;}="";public string? Description{get;set;}public ICollection<Post> Posts{get;set;}=new List<Post>();}
public class Tag{public Guid Id{get;set;}=Guid.NewGuid();public string Name{get;set;}="";public string Slug{get;set;}="";public ICollection<PostTag> PostTags{get;set;}=new List<PostTag>();}
public class Post:BaseEntity{public string Title{get;set;}="";public string Slug{get;set;}="";public string? Summary{get;set;}public string Content{get;set;}="";public string? ThumbnailUrl{get;set;}public Guid AuthorId{get;set;}public User? Author{get;set;}public Guid CategoryId{get;set;}public Category? Category{get;set;}public PostStatus Status{get;set;}=PostStatus.Draft;public int ViewCount{get;set;}public DateTime? PublishedAt{get;set;}public ICollection<PostTag> PostTags{get;set;}=new List<PostTag>();public ICollection<Comment> Comments{get;set;}=new List<Comment>();}
public class PostTag{public Guid PostId{get;set;}public Post? Post{get;set;}public Guid TagId{get;set;}public Tag? Tag{get;set;}}
public class Comment:BaseEntity{public Guid PostId{get;set;}public Post? Post{get;set;}public Guid UserId{get;set;}public User? User{get;set;}public Guid? ParentCommentId{get;set;}public Comment? ParentComment{get;set;}public string Content{get;set;}="";public bool IsDeleted{get;set;}}
public class Like{public Guid PostId{get;set;}public Guid UserId{get;set;}public DateTime CreatedAt{get;set;}=DateTime.UtcNow;}
public class Bookmark{public Guid PostId{get;set;}public Guid UserId{get;set;}public DateTime CreatedAt{get;set;}=DateTime.UtcNow;}
public class RefreshToken:BaseEntity{public Guid UserId{get;set;}public string Token{get;set;}="";public DateTime ExpiresAt{get;set;}public bool Revoked{get;set;}}
