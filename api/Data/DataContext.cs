using api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
      public class DataContext : IdentityDbContext<AppUser, AppRole, int, 
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        { }
      
        public DbSet<UserLike> Likes {get; set;}
<<<<<<< HEAD
        public DbSet<Message> Messages {get; set;}
=======

        public DbSet<Message> Messages {get; set;}

>>>>>>> ead028757c5a25c5ef9bbfbfdb8724eddc91f1dd
        protected override void OnModelCreating(ModelBuilder builder)
        {  
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessageReceived)                
                .OnDelete(DeleteBehavior.Restrict);
<<<<<<< HEAD
=======

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m=>m.MessageSent)                
                .OnDelete(DeleteBehavior.NoAction);

>>>>>>> ead028757c5a25c5ef9bbfbfdb8724eddc91f1dd

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m=>m.MessageSent)                
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AppUser>(builder =>
            {
                builder.Property(x => x.DateOfBirth).HasConversion<DateOnlyConverter, DateOnlyComparer>();
            });
        }
    }
}