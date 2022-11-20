using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using api.DTOs;
using api.Interfaces;

namespace api.Controllers
{
    
    public class AccountController:BaseApiController
    {

        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto newUser)
        {
            if (await UserExists (newUser.Username))
            {
                return BadRequest("Username is taken");
            }
            using var hmac = new HMACSHA512();

            var user = new AppUser {
                UserName = newUser.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newUser.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto {
                Username =user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            //find if the user name exist
            //if exist get a hmac from its salt key
            //get user's password hash

            //for the login user find its hash by the same key
            //compare the user's passwordhash and login's password hash to see if they are the same
            var user = await _context.Users.SingleOrDefaultAsync(u=>u.UserName ==login.Username);
            if (user ==null)
            return Unauthorized("This login does not exist");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var calculatedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(login.Password));
            for (int i=0; i<calculatedPasswordHash.Length; i++)
            {
                if (calculatedPasswordHash[i]!=user.PasswordHash[i]) 
                return Unauthorized("user does not have right password");

            }
            return new UserDto {
                Username =user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }










        private async Task<bool> UserExists(string username)
        {
           return await _context.Users.AnyAsync(result=>result.UserName == username.ToLower());

        }

    }

}