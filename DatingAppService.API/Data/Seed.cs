﻿using DatingAppService.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DatingAppService.API.Data
{
	public class Seed
	{
		public static async Task SeedUsers(DataContext context)
		{
			if (await context.Users.AnyAsync()) return;

			var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

			var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

			foreach (var user in users)
			{
				user.UserName = user.UserName.ToLower();

				context.Users.Add(user);
			}

			await context.SaveChangesAsync();
		}
	}
}