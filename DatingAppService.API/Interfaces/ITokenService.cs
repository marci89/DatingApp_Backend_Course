using DatingAppService.API.Entities;

namespace DatingAppService.API.Interfaces
{
	public interface ITokenService
	{
		Task<string> CreateToken(AppUser user);
	}
}
