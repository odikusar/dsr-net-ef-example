//dotnet run
// http://localhost:5020/api/addresses
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AppTestDbContext _context;

        public AddressesController(AppTestDbContext context)
        {
            _context = context;
        }

        // GET api/addresses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return Ok(new { address.Id, address.Name });
        }

        // GET api/addresses - Get all addresses
        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var addresses = await _context.Addresses
                                          .Select(a => new { a.Id, Name = "Name: " + a.Name })
                                          .ToListAsync();

            return Ok(addresses);
        }
    }
}
