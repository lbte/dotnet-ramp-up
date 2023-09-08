using System;
using PRFTLatam.EmploymentInfo.Domain;
using PRFTLatam.EmploymentInfo.Domain.Models;

namespace PRFTLatam.EmploymentInfo.Application.Services;

public class DeveloperService : IDeveloperService
{
    private readonly IUnitOfWork _unitOfWork;

    public DeveloperService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Developer>> GetDevelopersAsync()
    {
        return await _unitOfWork.DeveloperRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Developer>> GetDevelopersByFirstName(string firstName)
    {
        return await _unitOfWork.DeveloperRepository.GetAllAsync(x => x.FirstName.ToLower().Equals(firstName.ToLower()), null, null);
    }

    public async Task<IEnumerable<Developer>> GetDevelopersByLastName(string lastName)
    {
        return await _unitOfWork.DeveloperRepository.GetAllAsync(x => x.LastName.ToLower().Equals(lastName.ToLower()), null, null);
    }

    public async Task<IEnumerable<Developer>> GetDevelopersByAge(int age)
    {
        return await _unitOfWork.DeveloperRepository.GetAllAsync(x => x.Age == age, null, null);
    }

    public async Task<IEnumerable<Developer>> GetDevelopersByWorkedHours(int workedHours)
    {
        return await _unitOfWork.DeveloperRepository.GetAllAsync(x => x.WorkedHours == workedHours, null, null);
    }

    public async Task<Developer> GetDeveloperByEmail(string email)
    {
        return await _unitOfWork.DeveloperRepository.FindAsync(email);
    }

    public async Task<Developer> CreateDeveloper(Developer developer)
    {
        if (developer.Age <= 10 || developer.WorkedHours <= 30 || developer.WorkedHours >= 50 || developer.SalaryByHours <= 13 || developer.FirstName.Length < 3 || developer.LastName.Length < 3 || developer.FirstName.Length > 20 || developer.LastName.Length > 30 || (Enum.IsDefined(typeof(DevelopersType), developer.DeveloperType) == false))
        {
            return null;
        }

        developer.FullName = $"{developer.FirstName} {developer.LastName}";

        await _unitOfWork.DeveloperRepository.AddAsync(developer);
        await _unitOfWork.SaveAsync();
        return developer;

    }

    public async Task<Developer> UpdateDeveloper(Developer developer)
    {
        if (developer.Age <= 10 || developer.WorkedHours <= 30 || developer.WorkedHours >= 50 || developer.SalaryByHours <= 13 || developer.FirstName.Length < 3 || developer.LastName.Length < 3 || developer.FirstName.Length > 20 || developer.LastName.Length > 30 || (Enum.IsDefined(typeof(DevelopersType), developer.DeveloperType) == false))
        {
            return null;
        }

        await _unitOfWork.DeveloperRepository.Update(developer);
        await _unitOfWork.SaveAsync();

        return developer;
    }

    public async Task<string> DeleteDeveloper(string email)
    {
        try
        {
            await _unitOfWork.DeveloperRepository.Delete(email);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return "";
        }
        return $"{email}";
    }
}