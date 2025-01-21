using AutoMapper;
using Demo.DataAccess.Models;
using Demo.Presentation.ViewModels;

namespace Demo.Presentation.MappingProfiles
{
    public class UserProfile :Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
        }
    }
}
