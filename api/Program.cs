using api.Data;
using api.Extensions;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using api.SignalR;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
// builder.Services.AddToRoleAsync(builder.Configuration);


var app = builder.Build();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
    .WithOrigins("https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try 
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    //context.Connections.RemoveRange(context.Connections);
    await context.Database.ExecuteSqlRawAsync("Truncate table [Connections]");
    await Seed.SeedUsers(userManager, roleManager );
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration, hello pay attention here");
}

app.Run();




