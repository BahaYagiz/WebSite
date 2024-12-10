using AutoMapper;
using WebSite.Models;
using WebSite.ViewModels;

namespace WebSite.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Report, ReportModel>().ReverseMap();
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<User, RegisterModel>().ReverseMap();
            CreateMap<Role, RoleModel>().ReverseMap(); 
        }
    }
}
