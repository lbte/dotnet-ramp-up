using PRFTLatam.EmploymentInfo.Domain.Models;

namespace PRFTLatam.EmploymentInfo.Application.Services;

public interface IDeveloperService
{
    Task <IEnumerable<Developer>> GetDevelopersAsync();
    Task <IEnumerable<Developer>> GetDevelopersByFirstName(string firstName);
    Task <IEnumerable<Developer>> GetDevelopersByLastName(string lastName);
    Task <IEnumerable<Developer>> GetDevelopersByAge(int age);
    Task <IEnumerable<Developer>> GetDevelopersByWorkedHours(int workedHours);
    Task <Developer> GetDeveloperByEmail(string email);
    Task <List<string>> CreateDeveloper(Developer developer);
    Task <List<string>> UpdateDeveloper(Developer developer);
    Task <string> DeleteDeveloper(string email);


}