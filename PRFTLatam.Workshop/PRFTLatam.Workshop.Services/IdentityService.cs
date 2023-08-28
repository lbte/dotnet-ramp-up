using System.Text.RegularExpressions;
using PRFTLatam.Workshop.Infrastructure;

namespace PRFTLatam.Workshop.Services;

public class IdentityService : IIdentityService
{
    private readonly FileParsingService _csvFile;

    public IdentityService(FileParsingService csvFile)
    {
        _csvFile = csvFile;
    }

    public List<string> GetIds()
    {
        return _csvFile.GetDataFromCSVFile(_csvFile.csvFilePath);
    }
    public List<string> IdentityValidation(List<string> ids)
    {
        // string to store the validation errors
        var errors = new List<string>();

        foreach (var id in ids)
        {
            if (id == null || id.Length == 0) 
            {
                errors.Add("Id can not be empty");
                return errors;
            }
            if (id.Length < 10 || id.Length > 32)
                errors.Add($"Id {id} must have a length between 10 and 32 characters");
            // Only hexadecimal numbers are supported: A-F 0-9
            Match m = Regex.Match(id, @"\b[A-F0-9]*\b");
            if (m.Length == 0)
                errors.Add($"Id {id} must have characters between A-F and 0-9");
        }
        return errors;
    }

    public List<string> AmountOfIdsIdentityValidation(int amount)
    {   
        var result = new List<string>();
        try
        {
            var ids = GetIds().GetRange(0, amount);
            result = IdentityValidation(ids);
        }
        catch (Exception e)
        {
            result.Add($"The amount of ids to check ({amount}) exceeds the available ids amount: {GetIds().Count}");
        } 
        return result;
    }
}