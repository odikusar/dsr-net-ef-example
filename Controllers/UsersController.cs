//dotnet run
// http://localhost:5020/api/users
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppTestDbContext _context;

        public UsersController(AppTestDbContext context)
        {
            _context = context;
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new { user.Id, user.FirstName, user.LastName, Address = user?.Address?.Name });
        }

        // GET api/users - Get all users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Include(u => u.Address)
                .Select(a => new { a.Id, Name = "Name: " + a.FirstName + " " + a.LastName, Address = a.Address.Name })
                .ToListAsync();

            return Ok(users);
        }
    }
}
