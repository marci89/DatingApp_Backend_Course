using Microsoft.AspNetCore.Identity;

namespace DatingAppService.API.Entities
{
	public class AppRole : IdentityRole<int>
	{
		public ICollection<AppUserRole> UserRoles { get; set; }
	}
}
