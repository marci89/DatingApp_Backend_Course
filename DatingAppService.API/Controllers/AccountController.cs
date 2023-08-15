using AutoMapper;
using DatingAppService.API.Data;
using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using DatingAppService.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppService.API.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly DataContext _context;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
		{
			_context = context;
			_tokenService = tokenService;
			_mapper = mapper;
		}


		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto request)
		{
			if (await UserExists(request.UserName))
				return BadRequest("UserName is taken");

			var user = _mapper.Map<AppUser>(request);

			user.UserName = request.UserName;

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return new UserDto
			{
				UserName = user.UserName,
				Token = _tokenService.CreateToken(user),
				KnownAs = user.KnownAs,
				Gender = user.Gender
			};
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto request)
		{
			var user = await _context.Users
				.Include(p => p.Photos)
				.SingleOrDefaultAsync(x => x.UserName == request.UserName);

			if (user == null) return Unauthorized("Invalid username");


			return new UserDto
			{
				UserName = user.UserName,
				Token = _tokenService.CreateToken(user),
				PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
				KnownAs = user.KnownAs,
				Gender = user.Gender
			};
		}

		private async Task<bool> UserExists(string userName)
		{
			return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
		}
	}
}
