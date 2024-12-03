namespace Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public required string ProductName { get; set; }
}
