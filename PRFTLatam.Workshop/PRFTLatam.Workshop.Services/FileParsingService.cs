using PRFTLatam.Workshop.Infrastructure;
using Microsoft.VisualBasic.FileIO;

namespace PRFTLatam.Workshop.Services;
public class FileParsingService : IFileParsing
{
    public string csvFilePath = @"C:\Users\laura.bustamanteh\OneDrive - Perficient, Inc\First Documents\dotnet-training\PRFTLatam.Workshop\PRFTLatam.Workshop.Infrastructure\Files\ids.csv";
    
    public List<string> GetDataFromCSVFile(string csvFilePath)
    {
        var ids = new List<string>();
        using(TextFieldParser csvReader = new TextFieldParser(csvFilePath))
        {
            csvReader.SetDelimiters(new string[] { "," });
            csvReader.HasFieldsEnclosedInQuotes = false;
            while (!csvReader.EndOfData)
            {
                try
                {
                    string?[] columns = csvReader.ReadFields();
                    foreach (var columnField in columns)
                    {
                        ids.Add(columnField);
                    }
                }
                catch (Exception e)
                {
                }
            }
        }
        return ids;
    }
}
