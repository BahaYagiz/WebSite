using AutoMapper;
using WebSite.ViewModels;
using WebSite.Models;
using WebSite.Repositories;

namespace WebSite.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}