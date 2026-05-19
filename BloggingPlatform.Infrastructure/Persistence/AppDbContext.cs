using BloggingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace BloggingPlatform.Infrastructure.Persistence;
public class AppDbContext:DbContext{
public AppDbContext(DbContextOptions<AppDbContext> o):base(o){}
public DbSet<User> Users=>Set<User>(); public DbSet<Post> Posts=>Set<Post>(); public DbSet<Category> Categories=>Set<Category>(); public DbSet<Tag> Tags=>Set<Tag>(); public DbSet<PostTag> PostTags=>Set<PostTag>(); public DbSet<Comment> Comments=>Set<Comment>(); public DbSet<Like> Likes=>Set<Like>(); public DbSet<Bookmark> Bookmarks=>Set<Bookmark>(); public DbSet<RefreshToken> RefreshTokens=>Set<RefreshToken>();
protected override void OnModelCreating(ModelBuilder b){b.Entity<User>().HasIndex(x=>x.Email).IsUnique();b.Entity<User>().HasIndex(x=>x.Username).IsUnique();b.Entity<Post>().HasIndex(x=>x.Slug).IsUnique();b.Entity<Category>().HasIndex(x=>x.Slug).IsUnique();b.Entity<Tag>().HasIndex(x=>x.Slug).IsUnique();b.Entity<PostTag>().HasKey(x=>new{x.PostId,x.TagId});b.Entity<Like>().HasKey(x=>new{x.PostId,x.UserId});b.Entity<Bookmark>().HasKey(x=>new{x.PostId,x.UserId});}
}
