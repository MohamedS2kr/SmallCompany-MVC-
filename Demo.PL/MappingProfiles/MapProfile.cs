using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Helper
{
    public class MapProfile :Profile
    {
        public MapProfile() 
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
            //CreateMap<EmployeeViewModel, Employee>();
            CreateMap<Department, DepartmentViewModel>().ReverseMap();

            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();

            CreateMap<IdentityRole, RoleViewModel>().ForMember(D=>D.RoleName,O=>O.MapFrom(S=>S.Name)).ReverseMap();
        }

    }
}
