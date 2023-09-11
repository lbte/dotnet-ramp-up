// public class Developer
// {
//     public string Name { get; set; }
//     public DevelopersType DeveloperType { get; set; } 
//     public int WorkedHours { get; set; }
//     public int SalaryByHours { get; set; }
// }

// public enum DevelopersType
// {
//     Junior,
//     Intermediate,
//     Senior,
//     Lead
// }

class Program
{
    static string filePath = @"C:\Users\laura.bustamanteh\Downloads\dotnet-ramp-up\PRFTLatam.DeveloperSalaryCalculator\developers.csv";

    static void Main()
    {
        var developers = ReadData(filePath);
        ShowDeveloperInfo(developers);
        ShowSalary(developers);
        
    }

    static void ShowDeveloperInfo(List<List<string>> developers)
    {
        foreach (var dev in developers)
        {
            Console.WriteLine($"Dev Name: {dev[0]}\nDev Type: {dev[1]}\nWorked Hours: {dev[2]}\nSalaryByHour: {dev[3]} USD\n");
        }
    }

    static List<List<string>> ReadData(string filePath)
    {
        List<List<string>> developers = new List<List<string>>();
        using (var reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                List<string> developer = line.Split(",").ToList();
                developers.Add(developer);
            }
        }
        developers.RemoveAt(0);
        return developers;
    }

    static void ShowSalary(List<List<string>> developers)
    {
        decimal totalSalary = 0;
        int totalHours = 0;
        int totalDevs = 0;
        foreach (var dev in developers)
        {
            totalSalary += CalculateSalary(dev[1], int.Parse(dev[2]), int.Parse(dev[3]));
            totalHours += int.Parse(dev[2]);
            totalDevs++;
        }

        Console.WriteLine($"\nResume:\nTotal Salary: {totalSalary} USD\nTotal Hours: {totalHours}\nTotal Devs: {totalDevs}");
    }

    static decimal CalculateSalary(string devType, int workedHours, int salaryByHour)
    {
        decimal baseSalary = workedHours * salaryByHour;
        decimal salary = 0;

        switch (devType)
        {
            case "Junior":
                salary = baseSalary;
                break;
            case "Intermediate":
                salary = baseSalary * 1.12m;
                break;
            case "Senior":
                salary = baseSalary * 1.25m;
                break;
            case "Lead":
                salary = baseSalary * 1.5m;
                break;
        }

        return salary;
    }
}