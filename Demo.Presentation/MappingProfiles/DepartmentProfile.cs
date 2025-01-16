using AutoMapper;
using Demo.DataAccess.Models;
using Demo.Presentation.ViewModels;

namespace Demo.Presentation.MappingProfiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel,Department>().ReverseMap();
        }
    }
}
