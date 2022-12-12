using api.Entities;
using Microsoft.EntityFrameworkCore;


namespace api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }

        public DbSet<AppUser> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
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