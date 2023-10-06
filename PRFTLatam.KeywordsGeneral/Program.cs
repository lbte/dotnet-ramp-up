public class Program
{
    public static void Main(string[] args)
    {
        #region Challenge1Calls
        PrintUserInfo("John", 25, "test@test.com");

        SumNumbers("2,4,5,8,10");

        // example
        // var phrase = CurrencyToWord(85000, string.Empty);
        // Console.WriteLine($"\n85000: {phrase}");

        string inputSalary;
        do
        {
            Console.WriteLine("\nType the developer salary:");
            inputSalary = Console.ReadLine();
        } while(ValidateSalary(inputSalary) == false);

        Console.WriteLine($"\nThe developer salary in words is: {CurrencyToWord(int.Parse(inputSalary), string.Empty)}");
        #endregion

        #region Challenge2Calls
        Console.WriteLine($"Int Salary: {CalculateSalary(salaryByHour: 50, workedHours: 40)}");
        Console.WriteLine($"Decimal Salary: {CalculateSalary(salaryByHour: 71.34m, workedHours: 45)}");
        Console.WriteLine($"Float Salary: {CalculateSalary(salaryByHour: 63.0f, workedHours: 45)}");
        Console.WriteLine($"Double Salary: {CalculateSalary(salaryByHour: 84.6599, workedHours: 47)}");
        #endregion

    }

    #region Challenge1
    static void PrintUserInfo(string name, int age, string email)
    {
        Console.WriteLine($"\nname: {name}\nage: {age}\nemail: {email}");
    }

    static void SumNumbers(string numbers)
    {
        string[] numbersList = numbers.Trim().Split(",");
        int sum = 0;
        foreach (var number in numbersList)
        {
            sum += int.Parse(number);
        }
        Console.WriteLine($"\n{string.Join(" + ", numbersList)} = {sum}");
    }

    static bool ValidateSalary(string inputSalary)
    {
        int salary = 0;
        int.TryParse(inputSalary, out salary);
        return salary > 50000 ? true : false;
    }

    static string CurrencyToWord(int number, string word)
    {
        if (number / 1_000_000 != 0)
        {
            word = string.Concat(CurrencyToWord(number / 1_000_000, word), " million ");
            number %= 1_000_000;
        }

        if (number / 1_000 != 0)
        {
            word = string.Concat(CurrencyToWord(number / 1_000, word), " thousand ");
            number %= 1_000;
        }
  
        if (number / 100 != 0)
        {
            word = string.Concat(CurrencyToWord(number / 100, word), " hundred ");
            number %= 100;
        }

        word = NumberToWord(number, word);
  
        return word;
    }

    static string NumberToWord(int number, string words)
    {
        if (words != "") words += " ";

        var unitValues = new[]
        {
            "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve",
            "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
        };
        var tensValues = new[]
        { "", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        if (number >= 20)
        {
            words += tensValues[number / 10];
            if (number % 10 > 0) words += "-" + unitValues[number % 10];
        }
        else
            words += unitValues[number];

        return words; 
    }
    #endregion

    #region Challenge2
    static dynamic CalculateSalary(dynamic salaryByHour, dynamic workedHours)
    {
        return salaryByHour * workedHours;
    }
    
    // static int CalculateSalary(int salaryByHour, int workedHours)
    // {
    //     Console.WriteLine("\n----Entered Int calculation----");
    //     return salaryByHour * workedHours;
    // }

    // static decimal CalculateSalary(decimal salaryByHour, int workedHours)
    // {
    //     Console.WriteLine("\n----Entered Decimal calculation----");
    //     return salaryByHour * workedHours;
    // }

    // static float CalculateSalary(float salaryByHour, int workedHours)
    // {
    //     Console.WriteLine("\n----Entered Float calculation----");
    //     return salaryByHour * workedHours;
    // }

    // static double CalculateSalary(double salaryByHour, int workedHours)
    // {
    //     Console.WriteLine("\n----Entered Double calculation----");
    //     return salaryByHour * workedHours;
    // }
    #endregion
}