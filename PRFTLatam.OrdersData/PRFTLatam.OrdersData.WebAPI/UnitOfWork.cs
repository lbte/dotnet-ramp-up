using Microsoft.EntityFrameworkCore;
using PRFTLatam.OrdersData.Infrastructure.Models;
using PRFTLatam.OrdersData.Services;

namespace PRFTLatam.OrdersData.WebAPI;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly OrdersDataContext _context;
    private bool _disposed = false;
    private IRepository<Client> _clientRepository;
    private IRepository<Product> _productRepository;
    private IRepository<Order> _orderRepository;

    public UnitOfWork(OrdersDataContext context)
    {
        _context = context;
    }

    // Each repository property checks whether the repository already exists. 
    // If not, it instantiates the repository, passing in the context instance. 
    // As a result, all repositories share the same context instance.
    public IRepository<Client> ClientRepository
    { 
        get {
            _clientRepository ??= new Repository<Client>(_context);
            return _clientRepository;
        }
    }
    public IRepository<Product> ProductRepository
    { 
        get {
            _productRepository ??= new Repository<Product>(_context);
            return _productRepository;
        }
    }
    public IRepository<Order> OrderRepository
    { 
        get {
            _orderRepository ??= new Repository<Order>(_context);
            return _orderRepository;
        }
    }

    public async Task SaveAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            ex.Entries.Single().Reload();
        }
    }

// Like any class that instantiates a database context in a class variable, 
// the UnitOfWork class implements IDisposable and disposes the context.
#region IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                _context.DisposeAsync();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
    }
#endregion

}