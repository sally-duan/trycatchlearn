using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using api.Interfaces;
using AutoMapper;
using api.DTOs;
using System.Security.Claims;

namespace api.Controllers
{
     public class UsersController:BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository  userRepository, IMapper mapper)
        {
            _userRepository= userRepository;
            _mapper = mapper;
        }

       // [AllowAnonymous]
        [HttpGet]
        public async Task< ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
        //   var users =await _userRepository.GetUsersAsync(); 
        //   var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
        //   return Ok(usersToReturn);
        var users = await _userRepository.GetMembersAsync();
        return Ok(users);
       
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
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
        //   var user =  await _userRepository.GetUser(username);    
        //   return _mapper.Map<MemberDto>(user);
              return await _userRepository.GetMemberAsync(username);
        }


       
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
         var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
         var user = await _userRepository.GetUser(username);
         if (user ==null) return NotFound();
         _mapper.Map(memberUpdateDto, user);

         if (await _userRepository.SaveAllAsync()) return NoContent();
         return BadRequest("failed to update user");

        }


    }
}