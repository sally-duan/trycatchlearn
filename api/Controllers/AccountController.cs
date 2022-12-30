using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using api.DTOs;
using api.Interfaces;
using AutoMapper;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace api.Controllers
{
    
    public class AccountController:BaseApiController
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> user, ITokenService tokenService, IMapper mapper)
        {
            _userManager = user;
            _tokenService = tokenService;
            _mapper= mapper;
        }

        // POST: api/account/register?username=dave&password=pwd
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists (registerDto.Username))
            {
                return BadRequest("Username is taken");
            }
            var user = _mapper.Map<AppUser>(registerDto);    
            user.UserName = registerDto.Username.ToLower();     
           
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            
            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);
            
            return new UserDto {
                Username =user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender               
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {                 
            var user = await _userManager.Users.Include(user=>user.Photos).SingleOrDefaultAsync(u=>u.UserName ==login.Username);
            if (user ==null) return Unauthorized("Invalid user name");            
           
           var result = await _userManager.CheckPasswordAsync(user, login.Password);
           if (!result) return Unauthorized("Invalid password");
           
           
            return new UserDto {
                Username =user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl= user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender               
            };
        }

        private async Task<bool> UserExists(string username)
        {
           return await this._userManager.Users.AnyAsync(result=>result.UserName == username.ToLower());
        }
    }

}