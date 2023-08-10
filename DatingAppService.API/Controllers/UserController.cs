using DatingAppService.API.Entities;
using DatingAppService.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppService.API.Controllers
{
	[Authorize]
	public class UserController : BaseApiController
	{
		private readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
		{
			return Ok(await _userRepository.GetUsersAsync());
		}

		[HttpGet("{username}")]
		public async Task<ActionResult<AppUser>> GetUser(string username)
		{
			return await _userRepository.GetUserByUsernameAsync(username);
		}

	}
}
