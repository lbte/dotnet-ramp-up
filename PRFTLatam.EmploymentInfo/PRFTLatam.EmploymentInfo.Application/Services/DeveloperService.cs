using Newtonsoft.Json;
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
        return await _unitOfWork.DeveloperRepository.GetAllAsync(x => x.FirstName.ToLower().Equals(firstName.ToLower()), null, "");
    }

    public async Task<IEnumerable<Developer>> GetDevelopersByLastName(string lastName)
    {
        return await _unitOfWork.DeveloperRepository.GetAllAsync(x => x.LastName.ToLower().Equals(lastName.ToLower()), null, "");
    }

    public async Task<IEnumerable<Developer>> GetDevelopersByAge(int age)
    {
        return await _unitOfWork.DeveloperRepository.GetAllAsync(x => x.Age == age, null, "");
    }

    public async Task<IEnumerable<Developer>> GetDevelopersByWorkedHours(int workedHours)
    {
        return await _unitOfWork.DeveloperRepository.GetAllAsync(x => x.WorkedHours == workedHours, null, "");
    }

    public async Task<Developer> GetDeveloperByEmail(string email)
    {
        return await _unitOfWork.DeveloperRepository.FindAsync(email);
    }

    public async Task<List<string>> CreateDeveloper(Developer developer)
    {
        List<string> errors = new List<string>();
        if (developer.Age <= 10)
            errors.Add("Not avalid age, it must be greater than 10"); 
        else if (developer.WorkedHours <= 30 || developer.WorkedHours >= 50)
            errors.Add("Worked hours must be between 30 and 50 hours, exclusive"); 
        else if (developer.SalaryByHours <= 13)
            errors.Add("The salary by hours must be greater than 13");  
        else if (developer.FirstName.Length < 3 || developer.FirstName.Length > 20)
            errors.Add("The First name field must have between 3 and 20 characters");
        else if (developer.LastName.Length < 3 || developer.LastName.Length > 30)
            errors.Add("The Last name field must have between 3 and 30 characters");
        else if (Enum.IsDefined(typeof(DevelopersType), developer.DeveloperType) == false)
            errors.Add("The Developer time must be a valid type");

        developer.FullName = $"{developer.FirstName} {developer.LastName}";

        if (!errors.Any())
        {
            errors.Add(JsonConvert.SerializeObject(developer));
            await _unitOfWork.DeveloperRepository.AddAsync(developer);
            await _unitOfWork.SaveAsync();
        }

        return errors;

    }

    public async Task<List<string>> UpdateDeveloper(Developer developer)
    {
        List<string> errors = new List<string>();
        if (developer.Age <= 10)
            errors.Add("Not avalid age, it must be greater than 10"); 
        else if (developer.WorkedHours <= 30 || developer.WorkedHours >= 50)
            errors.Add("Worked hours must be between 30 and 50 hours, exclusive"); 
        else if (developer.SalaryByHours <= 13)
            errors.Add("The salary by hours must be greater than 13");  
        else if (developer.FirstName.Length < 3 || developer.FirstName.Length > 20)
            errors.Add("The First name field must have between 3 and 20 characters");
        else if (developer.LastName.Length < 3 || developer.LastName.Length > 30)
            errors.Add("The Last name field must have between 3 and 30 characters");
        else if (Enum.IsDefined(typeof(DevelopersType), developer.DeveloperType) == false)
            errors.Add("The Developer time must be a valid type");

        if (developer.FullName == null)
            developer.FullName = $"{developer.FirstName} {developer.LastName}";

        if (!errors.Any())
        {
            errors.Add(JsonConvert.SerializeObject(developer));
            await _unitOfWork.DeveloperRepository.Update(developer);
            await _unitOfWork.SaveAsync();
        }

        return errors;
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