﻿using DatingAppService.API.Data;
using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using DatingAppService.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingAppService.API.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly DataContext _context;
		private readonly ITokenService _tokenService;

		public AccountController(DataContext context, ITokenService tokenService)
		{
			_context = context;
			_tokenService = tokenService;
		}


		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto request)
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

			return new UserDto
			{
				UserName = user.UserName,
				Token = _tokenService.CreateToken(user)
			};
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto request)
		{
			var user = await _context.Users.SingleOrDefaultAsync(x =>
			x.UserName == request.UserName);

			if (user == null) return Unauthorized("Invalid username");

			using var hmac = new HMACSHA512(user.PasswordSalt);

			var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

			for (int i = 0; i < computedHash.Length; i++)
			{
				if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");

			}

			return new UserDto
			{
				UserName = user.UserName,
				Token = _tokenService.CreateToken(user)
			};
		}

		private async Task<bool> UserExists(string userName)
		{
			return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
		}
	}
}
