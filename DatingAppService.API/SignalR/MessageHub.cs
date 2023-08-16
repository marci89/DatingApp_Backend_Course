﻿using AutoMapper;
using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using DatingAppService.API.Extensions;
using DatingAppService.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingAppService.API.SignalR
{
	[Authorize]
	public class MessageHub : Hub
	{
		private readonly IMessageRepository _messageRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IHubContext<PresenceHub> _presenceHub;

		public MessageHub(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper, IHubContext<PresenceHub> presenceHub)
		{
			_messageRepository = messageRepository;
			_userRepository = userRepository;
			_mapper = mapper;
			_presenceHub = presenceHub;
		}

		public override async Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var otherUser = httpContext.Request.Query["user"];
			var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
			await AddToGroup(groupName);

			var messages = await _messageRepository
				.GetMessageThread(Context.User.GetUsername(), otherUser);


			await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
		}

		public override async Task OnDisconnectedAsync(Exception ex)
		{
			await RemoveFromMessageGroup();

			await base.OnDisconnectedAsync(ex);
		}

		public async Task SendMessage(CreateMessageDto createMessageDto)
		{
			var username = Context.User.GetUsername();

			if (username == createMessageDto.RecipientUsername.ToLower())
				throw new HubException("You cannot send messages to yourself");

			var sender = await _userRepository.GetUserByUsernameAsync(username);
			var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

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

			var group = await _messageRepository.GetMessageGroup(groupName);

			if (group.Connections.Any(x => x.Username == recipient.UserName))
			{
				message.DateRead = DateTime.UtcNow;
			}


			_messageRepository.AddMessage(message);

			if (await _messageRepository.SaveAllAsync())
			{
				await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
			}
		}



		private string GetGroupName(string caller, string other)
		{
			var stringCompare = string.CompareOrdinal(caller, other) < 0;
			return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
		}

		private async Task AddToGroup(string groupName)
		{
			var group = await _messageRepository.GetMessageGroup(groupName);
			var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

			if (group == null)
			{
				group = new Group(groupName);
				_messageRepository.AddGroup(group);
			}

			group.Connections.Add(connection);

			await _messageRepository.SaveAllAsync();
		}

		private async Task RemoveFromMessageGroup()
		{
			var connection = await _messageRepository.GetConnection(Context.ConnectionId);

			_messageRepository.RemoveConnection(connection);

			await _messageRepository.SaveAllAsync();
		}
	}
}