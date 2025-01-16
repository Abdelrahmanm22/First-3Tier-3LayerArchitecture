using AutoMapper;
using Demo.DataAccess.Models;
using Demo.Presentation.ViewModels;

namespace Demo.Presentation.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            ///if you need change names of data
            ///CreateMap<EmployeeViewModel, Employee>().ForMember(e=>e.Name,ev=>ev.MapFrom(s=>s.EmpName));
        }
    }
}
