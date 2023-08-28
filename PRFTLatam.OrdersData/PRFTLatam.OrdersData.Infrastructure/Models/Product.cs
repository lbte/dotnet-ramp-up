using System.ComponentModel.DataAnnotations;

namespace PRFTLatam.OrdersData.Infrastructure.Models;

public class Product : BaseEntity<int>
{
    /// <summary>
    /// Name of the product. Max 20 alphanumeric characters
    /// </summary>
    [MaxLength(20)]
    public string Name { get; set; } = "";
    /// <summary>
    /// Price of the product. Max 5 figures, it can have two decimal places
    /// </summary>
    [MaxLength(5)]
    public decimal Price { get; set; }
}