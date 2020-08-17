using AutoMapper;
using server.Dtos;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Entities.Identity;

namespace WebApi.Helpers {
	public class AutoMapperProfile : Profile {
		public AutoMapperProfile() {
			CreateMap<AppUser, UserDto>();
			CreateMap<UserDto, AppUser>();
			CreateMap<Campaigns, CampaignDto>();
			CreateMap<Dependents, DependentDto>();
		}
	}
}