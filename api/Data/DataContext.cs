using api.Entities;
using Microsoft.EntityFrameworkCore;


namespace api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }

        public DbSet<AppUser> Users { get; set; }

        public DbSet<UserLike> Likes {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {  
            base.OnModelCreating(builder);

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



            builder.Entity<AppUser>(builder =>
            {
                builder.Property(x => x.DateOfBirth).HasConversion<DateOnlyConverter, DateOnlyComparer>();

            });
          

        }










        // protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        // {
        //     builder.Properties<DateOnly>()
        //         .HaveConversion<DateOnlyConverter>()
        //         .HaveColumnType("date");
        // }
    }





}