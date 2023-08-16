using AutoMapper;
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
		private readonly IMapper _mapper;
		private readonly IHubContext<PresenceHub> _presenceHub;

		public MessageHub(IMessageRepository messageRepository, IMapper mapper, IHubContext<PresenceHub> presenceHub)
		{
			_messageRepository = messageRepository;
			_mapper = mapper;
			_presenceHub = presenceHub;
		}

		public override async Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var otherUser = httpContext.Request.Query["user"];
			var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

			var messages = await _messageRepository
				.GetMessageThread(Context.User.GetUsername(), otherUser);


			await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
		}

		public override Task OnDisconnectedAsync(Exception ex)
		{

			return base.OnDisconnectedAsync(ex);
		}


		private string GetGroupName(string caller, string other)
		{
			var stringCompare = string.CompareOrdinal(caller, other) < 0;
			return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
		}
	}
}
