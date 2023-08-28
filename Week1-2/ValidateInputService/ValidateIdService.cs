using System.Text.RegularExpressions;

namespace ValidateInputService.Services;
public class ValidateIdService
{
    // strings to test
    // var stringsToTest = new List<string>{ "A123456789", "D542", "FREGTHY547896321458934789fgtredjuyh", "fgurhty855", "", "547"};
    // var watch = new Stopwatch();
    // var elapsedMs = 0;

    // var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    // var cpuUsage = cpuCounter.NextValue();

    


    // method that validates an input with ifs
    // Input Parameters: A string that stores an id.
    // Output parameters: A list with the validation problems. If the id is correct, return an empty list.
    public List<string> validateInputIfs(string id)
    {
        // list of strings to store the validation errors
        var errors = new List<string>();

        if (id == null || id.Length == 0) 
        {
            errors.Add("Id can not be empty");
            return errors;
        }

        if (id.Length < 5 || id.Length > 32)
            errors.Add("Id must have a length between 5 and 32 characters");

        if (!char.IsUpper(id, 0))
            errors.Add("Id must start with a capital letter between A and Z");

        return errors;
    } 

    // method that validates an input with regular expressions
    // https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference
    public List<string> validateInputRegex(string id)
    {
        // string to store the validation errors
        var errors = new List<string>();

        if (id == null || id.Length == 0) 
        {
            errors.Add("Id can not be empty");
            return errors;
        }

        if (id.Length < 5 || id.Length > 32)
            errors.Add("Id must have a length between 5 and 32 characters");

        if (!Regex.Matches(id, "^[A-Z]+", RegexOptions.IgnorePatternWhitespace).Any())
            errors.Add("Id must start with a capital letter between A and Z");

        return errors;
    } 

    // method that validates an input with Throwing exceptions 
    // exceptions: https://www.tutorialsteacher.com/csharp/csharp-exception
    public void validateInputException(string id)
    {
        if (id == null || id.Length == 0) 
        {
            throw new ArgumentNullException("Id can not be empty");
        }

        if (id.Length < 5 || id.Length > 32)
            throw new ArgumentException("Id must have a length between 5 and 32 characters");

        if (!char.IsUpper(id, 0))
            throw new ArgumentException("Id must start with a capital letter between A and Z");
    }


    // watch.Start();
    // for (int i = 0; i < 100000; i++)
    // {
    //     validateInput(stringsToTest[0]);
    //     validateInput(stringsToTest[1]);
    //     validateInput(stringsToTest[2]);
    //     validateInput(stringsToTest[3]);
    // }
    // watch.Stop();
    // elapsedMs = watch.ElapsedMilliseconds;
    // Console.WriteLine($"Total execution time ifs: {elapsedMs} ms");

    // watch.Start();
    // for (int i = 0; i < 100000; i++)
    // {
    //     validateInputRegex(stringsToTest[0]);
    //     validateInputRegex(stringsToTest[1]);
    //     validateInputRegex(stringsToTest[2]);
    //     validateInputRegex(stringsToTest[3]);
    // }
    // watch.Stop();
    // elapsedMs = watch.ElapsedMilliseconds;
    // Console.WriteLine($"Total execution time Regex: {elapsedMs} ms");

    // watch.Start();
    // for (int i = 0; i < 100000; i++)
    // {
    //     try
    //     {
    //         validateInputException(stringsToTest[0]);
    //         validateInputException(stringsToTest[1]);
    //         validateInputException(stringsToTest[2]);
    //         validateInputException(stringsToTest[3]);
    //     }
    //     catch (Exception e)
    //     {
    //         continue;
    //     }
    // }
    // watch.Stop();
    // elapsedMs = watch.ElapsedMilliseconds;
    // Console.WriteLine($"Total execution time Exception: {elapsedMs} ms");

}
