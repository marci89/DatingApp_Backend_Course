using AutoMapper;
using DatingAppService.API.DTOs;
using DatingAppService.API.Entities;
using DatingAppService.API.Extensions;

namespace DatingAppService.API.Helpers
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<AppUser, MemberDto>()
		  .ForMember(dest => dest.PhotoUrl, opt =>
			  opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
		  .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

			CreateMap<Photo, PhotoDto>();
		}
	}
}
