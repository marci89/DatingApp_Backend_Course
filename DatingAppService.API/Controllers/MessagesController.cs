using AutoMapper;
using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using DatingAppService.API.Extensions;
using DatingAppService.API.Helpers;
using DatingAppService.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppService.API.Controllers
{
	[Authorize]
	public class MessagesController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IMessageRepository _messageRepository;
		private readonly IMapper _mapper;
		public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_messageRepository = messageRepository;
			_mapper = mapper;
		}

		[HttpPost]
		public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
		{
			var username = User.GetUsername();

			if (username == createMessageDto.RecipientUsername.ToLower())
				return BadRequest("You cannot send messages to yourself");

			var sender = await _userRepository.GetUserByUsernameAsync(username);
			var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

			if (recipient == null) return NotFound();

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

		[HttpGet]
		public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
		{
			messageParams.Username = User.GetUsername();

			var messages = await _messageRepository.GetMessagesForUser(messageParams);

			Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage,
				messages.PageSize, messages.TotalCount, messages.TotalPages));

			return messages;
		}
	}
}
