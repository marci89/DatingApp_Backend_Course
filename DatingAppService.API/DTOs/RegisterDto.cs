using System.ComponentModel.DataAnnotations;

namespace DatingAppService.API.DTOs
{
	public class RegisterDto
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
