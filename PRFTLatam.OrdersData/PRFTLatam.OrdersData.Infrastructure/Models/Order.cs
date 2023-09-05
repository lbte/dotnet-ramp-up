using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRFTLatam.OrdersData.Infrastructure.Models;

public class Order
{
    [Key]
    public long Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    /// <summary>
    /// The <see cref="Client"/> id this order belongs to
    /// </summary>
    [ForeignKey("Client")]
    public string ClientId { get; set; } = "";

    /// <summary>
    /// The <see cref="Product"/> id this order belongs to
    /// </summary>
    [ForeignKey("Product")]
    public int ProductId { get; set; }

    /// <summary>
    /// The <see cref="Product"/> entity this order is referring to
    /// </summary>
    public virtual Product? Product { get; set; }

    /// <summary>
    /// Quantity required in the order. Max 200
    /// </summary>
    public int RequiredQuantity { get; set; }

    /// <summary>
    /// Price at which the product was purchased when the order was placed
    /// </summary>
    public decimal Price { get; set; }
}