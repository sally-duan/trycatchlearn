using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using api.Interfaces;

namespace api.Controllers
{
     public class UsersController:BaseApiController
    {
        // private readonly DataContext _context;
        // public UsersController(DataContext context)
        // {
        //     _context= context;
        // }


       private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository  userRepository)
        {
            _userRepository= userRepository;
        }

       // [AllowAnonymous]
        [HttpGet]
        public async Task< ActionResult<IEnumerable<AppUser>>> GetUserAsync()
        {
            return Ok(await _userRepository.GetUserAsync());          
       
        }

        //api/users/3
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUserByIdAsync(int id)
        {
         return Ok(await _userRepository.GetUserByIdAsync(id));  
        }

        //api/users/username
        [Authorize]
        [HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
         return await _userRepository.GetUserByUsernameAsync(username);           
        }
    }
}