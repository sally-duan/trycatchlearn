
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Entities;


namespace api.Controllers
{
    public class AdminController:BaseApiController
    {

        private readonly UserManager<AppUser> _userManager;
        public AdminController (UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [Authorize(Policy="RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles(){
            var users = await _userManager.Users.OrderBy(u=>u.UserName)
            .Select(u=>new {
                u.Id,
                username = u.UserName,
                roles=u.UserRoles.Select(r=>r.Role.Name).ToList()
            }).ToListAsync();

            return Ok(users);
        }


        [Authorize(Policy="RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery]string roles)
        {
           if (string.IsNullOrEmpty(roles))
            return BadRequest("you must select at least one role");
          var selectedRoles = roles.Split(",").ToArray();
//to watch



          var user = await _userManager.FindByNameAsync(username);
          if (user ==null) return NotFound("there is no user!!!!");

          var userRoles = await _userManager.GetRolesAsync(user);
          var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

           if (!result.Succeeded) return BadRequest("failed to add to roles");

          result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
          if (!result.Succeeded) return BadRequest("failed to remove");

          return Ok(await _userManager.GetRolesAsync(user));
        
        }










        [Authorize(Policy="ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
              return Ok("Admins or moderators can see this.");
        }
    }

   
}