using System.ComponentModel.DataAnnotations;

namespace PRFTLatam.OrdersData.Infrastructure.Models;

public class Client
{
    [Key]
    [MaxLength(32)]
    public string Id { get; set; } = "";

    /// <summary>
    /// Name of the client. Max 50 characters
    /// </summary>
    [MaxLength(50)]
    public string Name { get; set; } = "";
    /// <summary>
    /// Client's quota in dollars. Max value 1.000.000. Also valid: 0.32 or 120.55
    /// </summary>
    public decimal Quota { get; set; } 

    public ICollection<Order>? Orders { get; set; }

    public decimal OrdersTotal { get; set; }
}