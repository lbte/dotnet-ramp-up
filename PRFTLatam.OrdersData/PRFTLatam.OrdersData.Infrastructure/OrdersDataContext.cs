using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using PRFTLatam.OrdersData.Infrastructure.Models;

namespace PRFTLatam.OrdersData.Infrastructure;

public class OrdersDataContext : DbContext
{
    public OrdersDataContext(DbContextOptions<OrdersDataContext> options) : base(options)
    {
    }

    // Collections of all the entities in the context for each class
    public DbSet<Client> Clients { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
}