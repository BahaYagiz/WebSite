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
            CreateMap<AppUser, UserModel>().ReverseMap();
            CreateMap<AppUser, RegisterModel>().ReverseMap();
            CreateMap<Todo, TodoModel>().ReverseMap();
        }
    }
}
