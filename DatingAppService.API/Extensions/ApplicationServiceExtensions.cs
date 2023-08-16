using DatingAppService.API.Data;
using DatingAppService.API.Helpers;
using DatingAppService.API.Interfaces;
using DatingAppService.API.Services;
using DatingAppService.API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DatingAppService.API.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{

			//DbContext
			services.AddDbContext<DataContext>(options =>
			{
				options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
			});


			services.AddCors();

			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IPhotoService, PhotoService>();

			services.AddScoped<LogUserActivity>();

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

			services.AddSignalR();
			services.AddSingleton<PresenceTracker>();

			services.AddScoped<IUnitOfWork, UnitOfWork>();


			return services;
		}
	}
}
