using AutoMapper;
using DevJobs.Application.Features.Membership.DTOs;
using DevJobs.Infrastructure.Features.Membership;

namespace DevJobs.Infrastructure
{
    public class InfrastructureProfile: Profile
    {
        public InfrastructureProfile()
        {
            CreateMap<UserDetailsDTO, ApplicationUser>()
                .ForMember(x=>x.ImageName, y=>y.Ignore())
                .ForMember(x => x.PasswordHash, y => y.Ignore())
                .ForMember(x => x.ConcurrencyStamp, y => y.Ignore())
                .ForMember(x => x.SecurityStamp, y => y.Ignore()).ReverseMap();

            CreateMap<UserCreateDTO, ApplicationUser>()
                .ForMember(x => x.ImageName, y => y.MapFrom(x => x.ImageUrl))
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.PhoneNumberConfirmed, y => y.Ignore())
                .ForMember(x => x.EmailConfirmed, y => y.Ignore())
                .ForMember(x => x.AccessFailedCount, y => y.Ignore())
                .ForMember(x => x.ConcurrencyStamp, y => y.Ignore())
                .ForMember(x => x.TwoFactorEnabled, y => y.Ignore())
                .ForMember(x => x.LockoutEnabled, y => y.Ignore())
                .ForMember(x => x.LockoutEnd, y => y.Ignore());

            CreateMap<ApplicationUser, UserListDTO>()
                .ForMember(x => x.UserId, y => y.MapFrom(x => x.Id))
                .ForMember(x => x.FirstName, y => y.MapFrom(x => x.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(x => x.LastName))
                .ForMember(x => x.UserName,  y => y.MapFrom(x => x.UserName))
                .ForMember(x=>x.ImageUrl, y=>y.MapFrom(x=>x.ImageName))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(x => x.PhoneNumber))
                .ForMember(x => x.UserEmail, y => y.MapFrom(x => x.Email));
        }
    }
}
