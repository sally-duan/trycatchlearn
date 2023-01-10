using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using api.Interfaces;
using AutoMapper;
using api.DTOs;
using System.Security.Claims;
using api.Extensions;
using api.Helpers;

namespace api.Controllers
{
     [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
         private readonly IUnitOfWork _uow;       
        private readonly IPhotoService _photoService;
        public UsersController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
           _uow = uow;
            _mapper = mapper;
            _photoService = photoService;
        }
       [HttpGet]
      // [Authorize(Roles="Admin")]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var gender = await _uow.UserRepository.GetUserGender(User.GetUsername());
            userParams.CurrentUsername =User.GetUsername();

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = gender == "male" ? "female" : "male";
            }

            var users = await _uow.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, 
                users.TotalCount, users.TotalPages));

            return Ok(users);
        }
        
        //api/users/3
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUserByIdAsync(int id)
        {
            return Ok(await _uow.UserRepository.GetUserByIdAsync(id));
        }

        //api/users/username
     //   [Authorize(Roles="Member")]
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {           
            return await _uow.UserRepository.GetMemberAsync(username);
        }


       


        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            
            if (user == null) return NotFound();
            _mapper.Map(memberUpdateDto, user);

            if (await _uow.Complete()) return NoContent();
            return BadRequest("failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            //above two lines can be shorted in one line like above method, using extension
            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.Photos.Count == 0)
                photo.IsMain = true;
            user.Photos.Add(photo);
            if (await _uow.Complete()) //return _mapper.Map<PhotoDto>(photo);
            {
                return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));

            }
            return BadRequest("problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
             var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("Already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null)
                currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _uow.Complete()) return NoContent();

            return BadRequest("Problem setting the main photo");
        }


        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeleteAllPhoto(int photoId)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain == true) return BadRequest("you can not delete your main photo");


            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if (await _uow.Complete()) return Ok();
            return BadRequest("Problem deleting photo");
        }

    }
}