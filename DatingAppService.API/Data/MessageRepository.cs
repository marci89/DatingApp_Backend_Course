using AutoMapper;
using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using DatingAppService.API.Helpers;
using DatingAppService.API.Interfaces;

namespace DatingAppService.API.Data
{
	public class MessageRepository : IMessageRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public MessageRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}


		public void AddMessage(Message message)
		{
			_context.Messages.Add(message);
		}

		public void DeleteMessage(Message message)
		{
			_context.Messages.Remove(message);
		}


		public async Task<Message> GetMessage(int id)
		{
			return await _context.Messages.FindAsync(id);
		}

		public Task<PagedList<MessageDto>> GetMessagesForUser()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserId, string recipientId)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;

		}
	}
}