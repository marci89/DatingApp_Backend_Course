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
		Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
		Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName);
		Task<bool> SaveAllAsync();

		void AddGroup(Group group);
		void RemoveConnection(Connection connection);
		Task<Connection> GetConnection(string connectionId);
		Task<Group> GetMessageGroup(string groupName);
		Task<Group> GetGroupForConnection(string connectionId);

	}
}
