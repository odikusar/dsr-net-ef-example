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
                .ThenInclude(u => u.Address)  //.Include(o => o.User.Address)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var result = new
            {
                OrderId = order.Id,
                UserFullName = $"{order.User?.FirstName} {order.User?.LastName}",
                ProductName = order.ProductName,
                AddressName = order.User != null && order.User.Address != null ? order.User.Address.Name : null
            };

            return result;
        }

        // GET api/orders?role={roleName}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOrders([FromQuery] string? role)
        {
            IQueryable<Order> query = _context.Orders
                .Include(o => o.User)
                .ThenInclude(u => u!.UserRoles)
                .Include(o => o.User)
                .ThenInclude(u => u!.Address);

            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(o => 
                    o.User != null && o.User.UserRoles.Any(ur => ur.Role.Name.ToLower() == role.ToLower())
                );
            }

            var orders = await query
                .Select(order => new
                {
                    OrderId = order.Id,
                    UserFullName = order.User != null ? $"{order.User.FirstName} {order.User.LastName}" : null,
                    ProductName = order.ProductName,
                    AddressName = order.User != null && order.User.Address != null ? order.User.Address.Name : null
                })
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound();
            }

            return orders;
        }
    }
}
