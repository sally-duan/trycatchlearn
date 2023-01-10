using api.Data;
using api.Interfaces;
using Microsoft.AspNetCore.SignalR;
using api.DTOs;
using api.Extensions;
using api.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace api.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        private readonly IHubContext<PresenceHub> _presenceHub;
        public MessageHub(IUnitOfWork uow, IMapper mapper, IHubContext<PresenceHub> presenceHub)
        {
            _uow = uow;
            _mapper = mapper;
            _presenceHub = presenceHub;
        }

        public override async Task OnConnectedAsync()
        {
            var httpConext = Context.GetHttpContext();
            var otherUser = httpConext.Request.Query["user"];
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);
            var messages = await _uow.MessageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);

            if (_uow.HasChanges()) await _uow.Complete();
            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);

        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup");
            await base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}:{other}" : $"{other}:{caller}";
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("you can not send message to yourself");

            var sender = await _uow.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _uow.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };
            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            var group = await _uow.MessageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }

            else
            {
                var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new { username = sender.UserName, knownAs = sender.KnownAs });
                }
            }




            _uow.MessageRepository.AddMessage(message);

            if (await _uow.Complete())
            {
                // var group = GetGroupName(sender.UserName, recipient.UserName);
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _uow.MessageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Group(groupName);
                _uow.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);
            if (await _uow.Complete())
            {
                return group;
            }
            throw new HubException("failed to add to group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _uow.MessageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _uow.MessageRepository.RemoveConnection(connection);

            if (await _uow.Complete()) return group;

            throw new HubException("failed to remove from group");
        }

    }
}