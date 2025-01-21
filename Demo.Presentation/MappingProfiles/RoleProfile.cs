using AutoMapper;
using Demo.Presentation.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.Presentation.MappingProfiles
{
    public class RoleProfile : Profile
    {

        public RoleProfile() {
            CreateMap<IdentityRole, RoleViewModel>();
        }
    }
}
