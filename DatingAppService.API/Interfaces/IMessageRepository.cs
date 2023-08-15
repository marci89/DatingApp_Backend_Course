using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using DatingAppService.API.Helpers;

namespace DatingAppService.API.Interfaces
{
	public interface IMessageRepository
	{
		void AddMessage(Message message);
		void DeleteMessage(Message message);
		Task<Message> GetMessage(int id);
		Task<PagedList<MessageDto>> GetMessagesForUser();
		Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserId, string recipientId);
		Task<bool> SaveAllAsync();

	}
}
