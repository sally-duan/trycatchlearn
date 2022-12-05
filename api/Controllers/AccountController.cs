using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using api.DTOs;
using api.Interfaces;
using AutoMapper;

namespace api.Controllers
{
    
    public class AccountController:BaseApiController
    {

        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper= mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists (registerDto.Username))
            {
                return BadRequest("Username is taken");
            }
            var user = _mapper.Map<AppUser>(registerDto);
            using var hmac = new HMACSHA512();

           
            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
           
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto {
                Username =user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
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
      
            var user = await _context.Users.Include(user=>user.Photos).SingleOrDefaultAsync(u=>u.UserName ==login.Username);
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
                Token = _tokenService.CreateToken(user),
                PhotoUrl= user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs = user.KnownAs
               
            };

        }

        private async Task<bool> UserExists(string username)
        {
           return await _context.Users.AnyAsync(result=>result.UserName == username.ToLower());
        }

    }

}