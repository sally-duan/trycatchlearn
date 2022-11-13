using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
     public class UsersController:BaseApiController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context= context;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task< ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
         var users =_context.Users.ToListAsync();
          return await users;
        }

        //api/users/3
        [Authorize]
         [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetAUser(int id)
        {
         var user = _context.Users.FindAsync(id);   
         return await  user;
        }
    }
}