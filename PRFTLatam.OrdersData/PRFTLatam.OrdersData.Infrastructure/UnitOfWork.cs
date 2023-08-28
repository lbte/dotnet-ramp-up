using Microsoft.EntityFrameworkCore;
using PRFTLatam.OrdersData.Infrastructure.Models;

namespace PRFTLatam.OrdersData.Infrastructure;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly OrdersDataContext _context;
    private bool _disposed = false;
    private IRepository<int, Client> _clientRepository;
    private IRepository<int, Product> _productRepository;
    private IRepository<long, Order> _orderRepository;

    public UnitOfWork(OrdersDataContext context)
    {
        _context = context;
    }

    // Each repository property checks whether the repository already exists. 
    // If not, it instantiates the repository, passing in the context instance. 
    // As a result, all repositories share the same context instance.
    public IRepository<int, Client> ClientRepository
    { 
        get {
            _clientRepository ??= new Repository<int, Client>(_context);
            return _clientRepository;
        }
    }
    public IRepository<int, Product> ProductRepository
    { 
        get {
            _productRepository ??= new Repository<int, Product>(_context);
            return _productRepository;
        }
    }
    public IRepository<long, Order> OrderRepository
    { 
        get {
            _orderRepository ??= new Repository<long, Order>(_context);
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