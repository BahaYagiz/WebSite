using WebSite.Models;

namespace WebSite.Repositories
{
    public class TodoRepository : GenericRepository<Todo>
    {
        public TodoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
