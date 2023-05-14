using AutoMapper;
using Employee_Management_System.Model.DTO;
using Employee_Management_System.Model;

namespace Employee_Management_System
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();

            CreateMap<Designation, DesignationDTO>().ReverseMap();
            CreateMap<Designation, DesignationCreateDTO>().ReverseMap();
            CreateMap<Designation, DesignationUpdateDTO>().ReverseMap();

            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Department, DepartmentCreateDTO>().ReverseMap();
            CreateMap<Department, DepartmentUpdateDTO>().ReverseMap();

            CreateMap<Employees, EmployeesDTO>().ReverseMap();
            CreateMap<Employees, EmployeesCreateDTO>().ReverseMap();
            CreateMap<Employees, EmployeesUpdateDTO>().ReverseMap();
        }
    }
    }

