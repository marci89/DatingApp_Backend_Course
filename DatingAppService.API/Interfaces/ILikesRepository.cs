using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;

namespace DatingAppService.API.Interfaces
{
	public interface ILikesRepository
	{
		Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
		Task<AppUser> GetUserWithLikes(int userId);
		Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
	}
}
