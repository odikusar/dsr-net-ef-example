using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

[Table("Addresses")]  // Specify the table name here, it is optional NOT NEEDED IN USUAL CASE!!!!!
public class Address
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
