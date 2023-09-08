using PRFTLatam.EmploymentInfo.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace PRFTLatam.EmploymentInfo.Domain;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly EmploymentInfoContext _context;
    private bool _disposed = false;
    private IRepository<Developer> _developerRepository;

    public UnitOfWork(EmploymentInfoContext context)
    {
        _context = context;
    }

    public IRepository<Developer> DeveloperRepository
    {
        get {
            _developerRepository ??= new Repository<Developer>(_context);
            return _developerRepository;
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