using api.DTOs;
using api.Entities;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;



namespace api.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;  

        private readonly IMapper _mapper;
        public MessagesController(IUnitOfWork uow,  IMapper mapper)
        {
            _mapper = mapper;
            _uow = uow;
           
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreatMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();
            if (username == createMessageDto.RecipientUsername.ToLower()) return BadRequest("You can not send message to yourself.");

            var sender = await _uow.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _uow.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);
            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _uow.MessageRepository.AddMessage(message);
            if (await _uow.Complete()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Not able to save a message");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _uow.MessageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));
            return messages;
        }


// [HttpGet("thread/{username}")]
// public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
// {
//     var currentUserName = User.GetUsername();
   
//     return Ok( await _uow.MessageRepository.GetMessageThread(currentUserName, username) );
// }


[HttpDelete("{id}")]
public async Task<ActionResult> DeleteMessage(int id)
{
    var username =User.GetUsername();
    var message = await _uow.MessageRepository.GetMessage(id);
    if (message.SenderUsername !=username && message.RecipientUsername !=username)
        return Unauthorized();

    if (message.SenderUsername ==username) message.SenderDeleted = true;
    if (message.RecipientUsername ==username) message.RecipientDeleted = true;

    if (message.SenderDeleted  && message.RecipientDeleted)
    {
        _uow.MessageRepository.DeleteMessage(message);
    }
    if (await _uow.Complete()) return Ok();
    return BadRequest("problem deleting the message");
}












    

    }
}