//dotnet run
// http://localhost:5020/api/users
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppTestDbContext _context;

        public OrdersController(AppTestDbContext context)
        {
            _context = context;
        }

        // POST api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromForm] Order order)
        {
            order.Date = DateTime.UtcNow;
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // returns result from GET method
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // GET api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .ThenInclude(u => u.Address)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var result = new
            {
                OrderId = order.Id,
                UserFullName = order.User?.FirstName + " " + order.User?.LastName,
                ProductName = order.ProductName,
                AddressName = order.User?.Address?.Name,
            };

            return result;
        }
    }
}
