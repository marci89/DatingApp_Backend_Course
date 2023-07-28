using DatingAppService.API.Data;
using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingAppService.API.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly DataContext _context;

		public AccountController(DataContext context)
		{
			_context = context;
		}


		[HttpPost("register")]
		public async Task<ActionResult<AppUser>> Register([FromBody] RegisterDto request)
		{
			if (await UserExists(request.UserName))
				return BadRequest("UserName is taken");

			using var hmac = new HMACSHA512();

			var user = new AppUser
			{
				UserName = request.UserName,
				PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
				PasswordSalt = hmac.Key
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return user;
		}

		private async Task<bool> UserExists(string userName)
		{
			return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
		}
	}
}
