## Excercise: Create a WebAPi

* The solution must be uploaded to a repository.
* Create a WebAPi with the methods:
* GET verb, HealthCheck resource, always returns 200 OK, no text.
* GET to determine if an identity is valid.
* The expected response is:
    * 200 OK if valid.
    * 422 Unprocessable Entity and an object with the list of errors.


* The validation logic must reside in a separate class from the WebAPI.
* Logic classes must reside in a separate project from the WebAPI. -> on the services project

**An ID is valid if:**

* is not empty or null.
* Its length is between 10 and 32 characters.
* Only hexadecimal numbers are supported: A-F 0-9
* The id is found within a list defined in a CSV text file, with the structure:
    
    A0A0A0A0A0, A0A0A0A0A1, A0A0A0A0A2, A0A0A0A0A3, A0A0A0A0A4<endoffile>

* The project to consume/read the text file must be different from Logic and WebAPI. -> in the infrastructure folder


**Suggested structure:**

        SRC
              WebAPI

              Services
                      Logic
                      Models

              Infrastructure

                      Files


        TESTS
             Unit.Tests
             Integration.Tests
             Component.Tests

## Approach

* PRFTLatam. Workshop.Infrastructure: 
    - Create interface (IFileParsing) for the method that will return all the ids from the csv file
    
* PRFTLatam. Workshop.Services:
    - Create implentation (FileParsing) of the interface (IFileParsing) that returns the ids from the csv file https://stackoverflow.com/questions/1405038/reading-a-csv-file-in-net 
    - Create interface (IIdentityService) for the id validation method
    - Create implementation (IdentityService) for that interface
    - Reference PRFTLatam. Workshop.Infrastructure

*  PRFTLatam. Workshop.WebAPI:
    - Create controller (IdentityController) to get the list of errors from the validation method in services
    - Create HealthCheck controller (HealthController)
    - Reference PRFTLatam. Workshop.Services

    - On Program.cs make dependency injection with both interfaces and implementations

## For the creation of the project:

1. Use the following commands to create the proyect

        mkdir PRFTLatam.Workshop
        cd .\PRFTLatam.Workshop\
        dotnet new sln -n PRFTLatam.Workshop
        dotnet new webapi -o PRFTLatam.Workshop.WebAPI
        dotnet new classlib -o PRFTLatam.Workshop.Services
        dotnet new classlib -o PRFTLatam.Workshop.Infrastructure
        dotnet sln add PRFTLatam.Workshop.WebAPI/
        dotnet sln add PRFTLatam.Workshop.Services/
        dotnet sln add PRFTLatam.Workshop.Infrastructure/
        dotnet build

        Build succeeded.
        0 Warning(s)
        0 Error(s)

2. Add references:

        dotnet add .\PRFTLatam.Workshop.Services\ reference .\PRFTLatam.Workshop.Infrastructure\
        dotnet add .\PRFTLatam.Workshop.WebAPI\ reference .\PRFTLatam.Workshop.Services\