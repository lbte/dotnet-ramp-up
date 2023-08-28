using Microsoft.VisualBasic.FileIO;
using System;

namespace PRFTLatam.Workshop.Infrastructure;

public interface IFileParsing
{
    
    List<string> GetDataFromCSVFile(string csvFilePath);
}