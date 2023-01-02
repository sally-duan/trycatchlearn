using api.Data;
using api.Interfaces;
using Microsoft.AspNetCore.SignalR;
using api.DTOs;
using api.Extensions;
using api.Entities;
using AutoMapper;

namespace api.SignalR
{
    public class MessageHub:Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
         private readonly IMapper _mapper;
        public MessageHub(api.Interfaces.IMessageRepository messageRepository, IUserRepository userRepository,
        IMapper mapper)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
           var httpConext = Context.GetHttpContext();
           var otherUser =httpConext.Request.Query["user"];
           var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
           await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

           var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);

           await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);

        } 
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}:{other}" : $"{other}-{caller}";
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
               throw new HubException("you can not send message to yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null)  throw new HubException("Not found user");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveAllAsync()) 
            {
                var group = GetGroupName(sender.UserName, recipient.UserName);
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }         
        }
    }
}