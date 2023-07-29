using Microsoft.OpenApi.Models;

namespace DatingAppService.API.Extensions
{
	public static class SwaggerServiceExtensions
	{
		public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
		{

			// Add Swagger generation
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

				// Add the security definition for bearer tokens
				c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				// Add the security Requirement for bearer tokens
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Name = "Bearer",
							In = ParameterLocation.Header,
							Reference = new OpenApiReference
								{
								Id = "Bearer",
								Type =ReferenceType.SecurityScheme
								}
						},
						new List<string>()
					}
				});
			});

			return services;
		}
	}
}
