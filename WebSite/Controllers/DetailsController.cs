using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSite.Models;

public class DetailsController : Controller
{
    private readonly AppDbContext _context;

    public DetailsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int id)
    {
 
        var report = await _context.Reports
            .FirstOrDefaultAsync(r => r.Id == id);

        if (report == null)
        {
            return NotFound(); 
        }

        return View(report); 
    }
}
