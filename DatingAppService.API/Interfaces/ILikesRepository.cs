using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using DatingAppService.API.Helpers;

namespace DatingAppService.API.Interfaces
{
	public interface ILikesRepository
	{
		Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
		Task<AppUser> GetUserWithLikes(int userId);
		Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
	}
}
