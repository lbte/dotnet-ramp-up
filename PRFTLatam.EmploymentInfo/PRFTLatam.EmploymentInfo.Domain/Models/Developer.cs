namespace PRFTLatam.EmploymentInfo.Domain.Models;

public class Developer
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FullName { get; set; } = "";
    public int Age { get; set; }
    public int WorkedHours { get; set; }
    public decimal SalaryByHours { get; set; }
    public DevelopersType DeveloperType { get; set; } = DevelopersType.Junior;
    public string Email { get; set; } = "";

}

public enum DevelopersType
{
    Junior,
    Intermediate,
    Senior,
    Lead
}