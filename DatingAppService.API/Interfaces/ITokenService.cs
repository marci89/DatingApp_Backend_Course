using DatingAppService.API.Entities;

namespace DatingAppService.API.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(AppUser user);
	}
}
