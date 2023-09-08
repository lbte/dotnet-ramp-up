using PRFTLatam.EmploymentInfo.Domain.Models;

namespace PRFTLatam.EmploymentInfo.Domain;

public interface IUnitOfWork
{
    IRepository<Developer> DeveloperRepository { get; }
    Task SaveAsync();
}