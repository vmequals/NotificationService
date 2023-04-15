using AutoMapper;
using NotificationService.Data.DTOs;
using NotificationService.Models;

namespace NotificationService.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<Subject, SubjectDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Team, TeamDTO>().ReverseMap();
        }
    }
}
