using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using DatingAppService.API.Helpers;

namespace DatingAppService.API.Interfaces
{
	public interface IUserRepository
	{
		void Update(AppUser user);
		Task<IEnumerable<AppUser>> GetUsersAsync();
		Task<AppUser> GetUserByIdAsync(int id);
		Task<AppUser> GetUserByUsernameAsync(string username);
		Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
		Task<MemberDto> GetMemberAsync(string username);

		Task<string> GetUserGender(string username);
	}
}
