﻿using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using DatingAppService.API.Extensions;
using DatingAppService.API.Helpers;
using DatingAppService.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppService.API.Controllers
{

	[Authorize]
	public class LikesController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly ILikesRepository _likesRepository;

		public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
		{
			_userRepository = userRepository;
			_likesRepository = likesRepository;
		}



		[HttpPost("{username}")]
		public async Task<ActionResult> AddLike(string username)
		{
			var sourceUserId = User.GetUserId();
			var likedUser = await _userRepository.GetUserByUsernameAsync(username);
			var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

			if (likedUser == null) return NotFound();

			if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

			var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

			if (userLike != null) return BadRequest("You already like this user");

			userLike = new UserLike
			{
				SourceUserId = sourceUserId,
				TargetUserId = likedUser.Id
			};

			sourceUser.LikedUsers.Add(userLike);

			if (await _userRepository.SaveAllAsync()) return Ok();

			return BadRequest("Failed to like user");
		}

		[HttpGet]
		public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
		{
			likesParams.UserId = User.GetUserId();
			var users = await _likesRepository.GetUserLikes(likesParams);

			Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize,
		   users.TotalCount, users.TotalPages));

			return Ok(users);
		}
	}
}