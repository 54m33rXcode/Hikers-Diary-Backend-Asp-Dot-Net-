
using Hikers_Diary.Models;
using HikersDiary_Web.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Configuration;


namespace HikersDiary_Web.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
        {
            
        }
        public DbSet<User> User { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Hashtag> Hashtag { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.Poster)
                .HasForeignKey(p => p.Poster_Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithOne(u => u.Follower)
                .HasForeignKey(f => f.Follower_Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Followings)
                .WithOne(u=>u.Following)
                .HasForeignKey(f=>f.Following_Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u=>u.Likes)
                .WithOne(l=>l.Liker)
                .HasForeignKey(f=>f.Liker_Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.Commentor)
                .HasForeignKey(f => f.Commentor_Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.SentNotifications)
                .WithOne(s => s.Sender)
                .HasForeignKey(f => f.Sender_Id);

            modelBuilder.Entity<User>()
                 .HasMany(u => u.ReceivedNotifications)
                .WithOne(s => s.Receiver)
                .HasForeignKey(f => f.Receiver_Id);

            modelBuilder.Entity<Post>()
                .HasMany(u => u.Likes)
                .WithOne(l => l.Post)
                .HasForeignKey(f => f.Lpost_Id);

            modelBuilder.Entity<Post>()
                .HasMany(u => u.Comments)
                .WithOne(l => l.Post)
                .HasForeignKey(f => f.Cpost_Id);

            modelBuilder.Entity<Post>()
                .HasMany(u => u.Categories)
                .WithOne(l => l.Post)
                .HasForeignKey(f => f.Catpost_Id);

            modelBuilder.Entity<Post>()
                .HasMany(u => u.Photoes)
                .WithOne(l => l.Post)
                .HasForeignKey(f => f.Ppost_Id);

            modelBuilder.Entity<Post>()
                .HasMany(u => u.Hashtags)
                .WithOne(l => l.Post)
                .HasForeignKey(f => f.Tpost_Id);

            modelBuilder.Entity<Comment>()
                .HasOne(u => u.ParentComment)
                .WithMany(c => c.ChildComments)
                .HasForeignKey(f => f.Parent_Cid);

            modelBuilder.Entity<User>()
                .HasKey(k => k.UserId);

            modelBuilder.Entity<Post>()
               .HasKey(k => k.Post_Id);

            modelBuilder.Entity<Comment>()
                .HasKey(k => k.C_Id);

            modelBuilder.Entity<Like>()
              .HasKey(k => k.Like_Id);

            modelBuilder.Entity<Photo>()
              .HasKey(k => k.Photo_Id);

            modelBuilder.Entity<Category>()
                .HasKey(k=>k.Category_Id);

            modelBuilder.Entity<Hashtag>()
                .HasKey(k => k.Tag_Id);

            modelBuilder.Entity<Follow>()
                 .HasKey(f => new { f.Follower_Id, f.Following_Id });

            modelBuilder.Entity<Notification>()
                 .HasKey(n => n.Notify_Id);

            

            base.OnModelCreating(modelBuilder);
        }
    }
}
