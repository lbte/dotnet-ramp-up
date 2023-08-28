using System.ComponentModel.DataAnnotations;

namespace PRFTLatam.OrdersData.Infrastructure.Models;

public class BaseEntity<TId> where TId : struct 
{
    /// <summary>
    /// Id value for each model, which will be the primary key
    /// </summary>
    [Key]
    public TId Id { get; set; }
}