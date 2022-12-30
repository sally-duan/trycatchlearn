using api.DTOs;
using api.Entities;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;



namespace api.Controllers
{
    public class MessageController: BaseApiController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
       
       private readonly IMapper _mapper;
       public MessageController(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper)
       {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _mapper = mapper;
       }

    //    [HttpPost]      
    //    public async Task<ActionResult<MessageDto>> CreatMessage(CreateMessageDto createMessageDto)
    //    {
    //     var username =  User.GetUsername();
    //     if (username ==createMessageDto.RecipientUsername.ToLower()) return BadRequest("you can not send message to yourself.");

    //     var sender = await _userRepository.GetUserByUsernameAsync(username);
    //     var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);
    //     if (recipient ==null) return NotFound();

    //     var message = new Message {
    //         Sender = sender,
    //         Recipient = recipient,
    //         SenderUsername = sender.UserName,
    //         RecipientUsername = recipient.UserName,
    //         Content = createMessageDto.Content
    //     };

    //     _messageRepository.AddMessage(message);
    //     if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

    //     return BadRequest("Not able to save a message");


    //    }
   


    [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send messages to yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound("Could not find Todd, this is impossible!!!");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }

















   
        // [HttpGet]
        // public async  Task<ActionResult<PagedList<MessageDto>>> GetUserMessages([FromQuery]MessageParams messageParams)
        // {
        //     // MessageParams.UserId = User.GetUserId();
        //     // var users = await _messageRepository.GetUserMessage(messageParams);      

        //     // Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize,
        //     //  users.TotalCount, users.TotalPages));            
        //     // return Ok(users);
        // }
   
    }
}