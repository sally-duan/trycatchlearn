using api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace api.Data
{
    public class DataContext: DbContext
    {
        public DataContext( DbContextOptions options):base(options)
        {

        }

        public DbSet<AppUser> Users {get; set;}
         protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");
        }
    }

       public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
      {
          public DateOnlyConverter() : base(
                  d => d.ToDateTime(TimeOnly.MinValue),
                  d => DateOnly.FromDateTime(d))
          { }
      }
}