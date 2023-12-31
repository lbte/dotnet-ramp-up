 # Week 1

 ## [Notification pattern](https://martinfowler.com/articles/replaceThrowWithNotification.html)

 A notification is an object that collects errors, each validation failure adds an error to the notification. A validation method returns a notification, which you can then interrogate to get more information. A simple usage looks has code like this for the checks.

    private void validateNumberOfSeats(Notification note) {
        if (numberOfSeats < 1) note.addError("number of seats must be positive");
        // more checks like this
    }

We can then have a simple call such as aNotification.hasErrors() to react if there are any errors. Other methods on the notification can drill into more details about the errors.

<img src="https://martinfowler.com/articles/replaceThrowWithNotification/sketch.png" width=700px>

I need to stress here, that I'm not advocating getting rid of exceptions throughout your code base. Exceptions are a very useful technique for handling exceptional behavior and getting it away from the main flow of logic. This refactoring is a good one to use only when the outcome signaled by the exception isn't really exceptional, and thus should be handled through the main logic of the program. 

### Building a Notification
In order to use a notification, you have to create the notification object. A notification can be really simple, sometimes just a list of strings will do the trick.

A Notification collects together errors

    List<String> notification = new ArrayList<>();
    if (numberOfSeats < 5) notification.add("number of seats too small");
    // do some more checks

    // then later…
    if ( ! notification.isEmpty()) // handle the error condition

Although a simple list idiom makes a lightweight implementation of the pattern, I usually like to do a bit more than this, creating a simple class instead.

    public class Notification {
        private List<String> errors = new ArrayList<>();

        public void addError(String message) { errors.add(message); }
        public boolean hasErrors() {
            return ! errors.isEmpty();
        }
        …

By using a real class, I can make my intention clearer - the reader doesn't have to perform the mental map between the idiom and its full meaning.

## Create a method to validate an input:

(For those of you who already have .NET Core experience, please use the new C# 9 instructions for simplified methods -Top Level Statement-)

Input Parameters: A string that stores an id.

Output parameters: A list with the validation problems. If the id is correct, return an empty list.

**Rules:**

* A valid ID must have a minimum of 5 characters, a maximum of 32.
* A valid ID must start with a capital letter: A-Z
* Implement the method in 3 different ways:

**Method 1:** Regular Expressions
**Method 2:** If normals.
**Method 3:** Throwing exceptions when some condition is not met.

* Create cycles of 1,000,000 interactions with each method.
* Within the loop, call the evaluated method 4 times: With a string without problem, with a string that does not meet the minimum length, with a string that does not meet the maximum length, with a string that does not meet the initial capitalization rule.
* Measure total execution time.
* Measure average and total processor and memory consumption by each method (System.Diagnostics) https://ourcodeworld.com/articles/read/1121/how-to-retrieve-the-amount-of-memory-used-within-your-own-c-sharp-winforms-application
* Show the execution times and memory/processor information of each method in console.

        using System;
        using System.Diagnostics;
        using System.Text.RegularExpressions;

        // method that validates an input with ifs
        // Input Parameters: A string that stores an id.
        // Output parameters: A list with the validation problems. If the id is correct, return an empty list.
        List<string> validateInput(string id)
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

            if (!char.IsUpper(id, 0))
                errors.Add("Id must start with a capital letter between A and Z");

            return errors;
        } 

        // method that validates an input with regular expressions
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference
        List<string> validateInputRegex(string id)
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
        void validateInputException(string id)
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

        // strings to test
        var stringsToTest = new List<string>{ "A123456789", "D542", "FREGTHY547896321458934789fgtredjuyh", "fgurhty855", "", "547"};

        // https://stackoverflow.com/questions/4679962/what-is-the-correct-performance-counter-to-get-cpu-and-memory-usage-of-a-process
        // PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", Process.GetCurrentProcess().ProcessName);
        // PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        // to measure the execution time https://www.tutorialsteacher.com/articles/how-to-calculate-code-execution-time-in-csharp
        foreach (var item in stringsToTest)
        {
            // var watch = new Stopwatch(); //Stopwatch.StartNew();
            // watch.Start();
            // validateInput(item);
            // watch.Stop();

            var validateId = validateInput(item);
            var validateIdRegex = validateInputRegex(item);

            // var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine($"\nString:{item}\nErrors:");
            foreach (var errorMessage in validateId)
            {
                Console.WriteLine($"- {errorMessage}");
            }

            Console.WriteLine($"Regex Errors:");
            foreach (var errorMessage in validateIdRegex)
            {
                Console.WriteLine($"- {errorMessage}");
            }

            Console.WriteLine($"Exception Errors:");
            try
            {
                validateInputException(item);
            }
            catch (Exception e)
            {
                Console.WriteLine($"- Processing failed: {e.Message}");
            }

            // Console.WriteLine($"Total execution time: {elapsedMs} ms");
        }

        // Console.WriteLine($"Total Processor time: {cpuCounter.NextValue()}");
        // Console.WriteLine($"Total Memory time: {ramCounter.NextValue()}");

# Week 2

## Code coverage

Code coverage is a software testing metric that determines the number of lines of code that is successfully validated under a test procedure, which in turn, helps in analyzing how comprehensively a software is verified.

Companies have to ensure that the software they develop meets all the essential quality characteristics – correctness, reliability, effectiveness, security, and maintainability. This can only be possible by thoroughly reviewing the software product.

Code coverage is one such software testing metric that can help in assessing the test performance and quality aspects of any software.

Such an insight will equally be beneficial to the development and QA team. For developers, this metric can help in dead code detection and elimination. On the other hand, for QA, it can help to check missed or uncovered test cases. 

To calculate the code coverage percentage, simply use the following formula:

    Code Coverage Percentage = (Number of lines of code executed by a testing algorithm/Total number of lines of code in a system component) * 100.

To measure the lines of code that are actually exercised by test runs, various criteria are taken into consideration. We have outlined below a few critical coverage criteria that companies use.

* Function Coverage – The functions in the source code that are called and executed at least once.
* Statement Coverage – The number of statements that have been successfully validated in the source code.
* Path Coverage – The flows containing a sequence of controls and conditions that have worked well at least once.
* Branch or Decision Coverage – The decision control structures (loops, for example) that have executed fine.
* Condition Coverage – The Boolean expressions that are validated and that executes both TRUE and FALSE as per the test runs.

Striking 100 percent code coverage means the code is 100 percent bugless. No error indicates that test cases have covered every criteria and requirement of the software application. So, if that’s the case, how do we evaluate if the test scripts have met a wide range of possibilities? What if the test cases have covered the incorrect requirements? What if test cases have missed on some important requirements? So, that drills down to the fact that, if a good software product built on 100 percent irrelevant test case coverage, then the software will undoubtedly compromise on quality.

### [Interactive Unit Testing with .NET Core and VS Code](https://www.codemag.com/Article/2009101/Interactive-Unit-Testing-with-.NET-Core-and-VS-Code)

1. Install the .NET Core Test Explorer extension

2. Create and Build a Class Library with Functions for Testing with the command

        dotnet new classlib -n Math

3. Add an XUnit Test Project and Create and Run a Unit Test via the Unit Test Explorer with the following command in the root folder:

        dotnet new xunit -n MathTests

    Before you can write unit tests against your code, the unit test project needs a reference to the math class library project. Adding project and NuGet references is a matter of adding the necessary entry to the csproj xml file.

    ![reference-math-class](media/reference-math-class.png)

### [Unit testing C# in .NET Core using dotnet test and xUnit](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)

1. Create the solution. in the root folder add the command:

        dotnet new sln -o UnitTests

2. Create a class lib inside of the folder UnitTests

        dotnet new classlib -o Math

    The dotnet new classlib command creates a new class library project in the Math folder. The new class library will contain the code to be tested.

3. Add the class library project to the solution:

        dotnet sln add ./Math/Math.csproj

4. Create the MathService.Tests project by running the following command:

        dotnet new xunit -o MathService.Tests

    The preceding command:

    * Creates the MathService.Tests project in the MathService.Tests directory. The test project uses [xUnit](https://xunit.net/) as the test library.
    * Configures the test runner by adding the following `<PackageReference />`elements to the project file:
        * `Microsoft.NET.Test.Sdk`
        * `xunit`
        * `xunit.runner.visualstudio`
        * `coverlet.collector`

5. Add the test project to the solution file by running the following command:

        dotnet sln add ./MathService.Tests/MathService.Tests.csproj

6. Add the MathService class library as a dependency to the MathService.Tests project:

        dotnet add ./MathService.Tests/MathService.Tests.csproj reference ./Math/Math.csproj

7. Create a test

    A popular approach in test driven development (TDD) is to write a (failing) test before implementing the target code. This tutorial uses the TDD approach. The IsPrime method is callable, but not implemented. A test call to IsPrime fails. With TDD, a test is written that is known to fail. The target code is updated to make the test pass. You keep repeating this approach, writing a failing test and then updating the target code to pass.

        using XUnit;
        using Math.Services;

        namespace Math.UnitTests.Services;

        public class MathService_IsPrimeShould
        {
            [Fact]
            public void IsPrime_InputIs1_ReturnFalse()
            {
                // Given
                var mathService = new MathService();

                // When
                bool result = mathService.IsPrime(1);

                // Then
                Assert.False(result, "1 should not be prime");
            }
        }

    The `[Fact]` attribute declares a test method that's run by the test runner. From the PrimeService.Tests folder, run `dotnet test`. The dotnet test command builds both projects and runs the tests. The xUnit test runner contains the program entry point to run the tests. dotnet test starts the test runner using the unit test project.

    The test fails because IsPrime hasn't been implemented. Using the TDD approach, write only enough code so this test passes. Update IsPrime with the following code:

        using System;

        namespace Math.Services;
        public class MathService
        {
            public bool IsPrime(int candidate)
            {
                if (candidate == 1)
                    return false;
                throw new NotImplementedException("Not fully implemented");
            }
        }

    Run dotnet test. The test passes.

8. Add more tests. Add prime number tests for 0 and -1.

    Copying test code when only a parameter changes results in code duplication and test bloat. The following xUnit attributes enable writing a suite of similar tests:

    * `[Theory]` represents a suite of tests that execute the same code but have different input arguments.
    * `[InlineData]` attribute specifies values for those inputs.

    Rather than creating new tests, apply the preceding xUnit attributes to create a single theory.

        // using XUnit;
        using Math.Services;

        namespace Math.UnitTests.Services;

        public class MathService_IsPrimeShould
        {
            private readonly MathService _mathService;

            public MathService_IsPrimeShould()
            {
                _mathService = new MathService();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            [InlineData(1)]
            public void IsPrime_ValueLessThan2_ReturnFalse(int value)
            {
                bool result = _mathService.IsPrime(value);

                // Then
                Assert.False(result, $"{value} should not be prime");
            }
        }

    Run dotnet test, and two of the tests fail. To make all of the tests pass, update the IsPrime method with the following code:

        using System;

        namespace Math.Services;
        public class MathService
        {
            public bool IsPrime(int candidate)
            {
                if (candidate < 2)
                    return false;
                throw new NotImplementedException("Not fully implemented");
            }
        }

## Exercise

For one of the methods implemented in week 1 create unit tests.
Use the `[Fact]` and `[Theory]` decorators and explain the difference.

        using ValidateInputService.Services;

        namespace ValidateInputService.Tests;

        public class UnitTest1
        {
        
            private readonly ValidateIdService _validateInput;

            public UnitTest1()
            {
                _validateInput = new ValidateIdService();
            }


            [Fact]
            public void CorrectId()
            {
                string id = "A123456789";
                var result = _validateInput.validateInputIfs(id);
                bool boolResult = result.Any();
                Assert.False(boolResult, "Id written correctly");

            }

            // https://learn.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2022
            [Fact]
            public void IncorrectId_LengthLessThanFive()
            {
                string id = "D542";
                var exception = Assert.Throws<ArgumentException>(() => _validateInput.validateInputException(id));
                Assert.Equal("Id must have a length between 5 and 32 characters", exception.Message);
            }

            [Fact]
            public void IncorrectId_LengthGreaterThanThirtyTwo()
            {
                string id = "FREGTHY547896321458934789fgtredjuyh";
                var result = _validateInput.validateInputIfs(id);
                bool boolResult = result.Any();
                Assert.True(boolResult, "Greater than 32 in length");
            }

            [Theory]
            [InlineData("fgurhty855")]
            [InlineData("547")]
            public void IncorrectId_NoUpperCaseLetter(string id)
            {
                var result = _validateInput.validateInputRegex(id);
                bool boolResult = result.Any();
                Assert.True(boolResult, "not fulfilling requirements");
            }
        }

# Week 3

## Web API

### REST (Representational State Transfer)

#### REST Constraints

1. **Client - Server**

    A client is the machine requesting a resource, and the server is the machine that responds with the requested resource because it holds them.
    * The client and the server must be separated, because of this they can evolve independently
    * Clients need not know anything about the business logic or data (which allows to process the request) access layer, and only need to worry about how to get the data and how to show it to the user 
    * And servers do not need to know anything about the frontend UI

2. **Stateless**

    A stateful architecture, remembers a clients activity between requests, throughout the duration of your time in the website

    A truly RESTFul architecture is not allowed to retain information about the state of another machine during the communication process.

    * The server should not store any session related client data. Everything that the server need to understand in respect to particular resource has to be contained within the particular request. Each request from a client to a server must be treated as if it was the first request the server has ever seen from that client. The client is free to store its session information in its own context.
    * A server should not remember its clients and readjust its state accordingly
    * The server may only give back the most up-to-date information about its state to the client and allow for modifications if the client is authorized to do so.
    * Stateless architecture improves scalability

    The downside of this is that as all the information has to be in the request, it needs more network badwidth to actually send a request because of the huge amount of data that the client is sending.

3. **Cacheable**

    Response messages from the server to the client are explicitly labeled as cacheable or non-cacheable, this way responses can be cached by the client if the information on the server hasn't changed since the last request 

    * When a server sends a response to the client, in this response it should indicate that whether the response can be cached or not and for how long can that happen at the client side. 

    * For subsequent requests, the client can retrieve from its cache, not needing to send a request to the server. It improves the network optimization

4. **Uniform interface**

    All restful architectures must have a uniform interface between all clients and servers. It means, a server must not require a different way of accesing data if a client is a windows laptop vs an iPhone or a Unix server. Gaining access to these endpoints is the same for any machine trying to access this information


5. **Layered system**

    It means that a client can have access to an endpoint that relies on other endpoints without having to know or understand all the underlying implementations. 

    Layering allows very complicated tasks to be completed tasks to be completed without having to understand all the underlying complexities that are required to generate a response.

    * Each layer doesn't knwo anything beyonds the immediate layer. Each layer is responsible for a specific function and only interacts with the layer next to it. Example: MVC framework:
        * Model: deals with data and the database
        * View: Only deals with how the output needs to be presented to the client
        * Controller: Only deals with the incomming user requests

    * It limits the complexity that can be introduced at any single layer as the layer are completely decoupled

    * One advantage is security. If an attack is made on a particular layer, the security breach may never reach the inner architectures because the layers are decoupled
    * One disadvantage is latency, for a particula request, it has to travel through several different layers to generate a response

6. **Code on Demand (optional)**

    It opens the possibility for code like JS from the server to be sent off to the client for execution.

    * In addition to data, the servers can also provide executable code to the client and the client can download it and execute it on the client side

## [Building Your First Web API with ASP.NET Core and Visual Studio Code](https://jasontaylor.dev/building-your-first-web-api-with-asp-net-core-and-visual-studio-code/) 

1. Create the project

        mkdir TodoApi
        cd TodoApi
        dotnet new webapi
        dotnet restore

2. Add a model class. Within the main project folder, add a folder named Models and then create a new file named TodoItem.cs.

        using System.ComponentModel.DataAnnotations;
        using System.ComponentModel.DataAnnotations.Schema;

        namespace TodoApi.Models;

        public class TodoItem
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long Key { get; set; }
            public string Name { get; set; }
            public bool IsComplete { get; set; }
        }

3. Add a database context and repository class. Update your TodoApi.csproj file to include a reference to the provider as follows:

    <img src="https://i0.wp.com/jasontaylor.dev/wp-content/uploads/2017/02/figure5_1.png?w=785&ssl=1" width=700px>

    Within the Models folder, add a new file named TodoContext.cs.

        using Microsoft.EntityFrameworkCore;

        namespace TodoApi.Models;

        public class TodoContext : DbContext
        {
            public TodoContext(DbContextOptions<TodoContext> options) : base(options)
            {
            }

            public DbSet<TodoItem> TodoItems { get; set; }
        }

    Within the Models folder, add a new file named ITodoRepository.cs.

        using System.Collections.Generic;

        namespace TodoApi.Models;

        public interface ITodoRepository
        {
            void Add(TodoItem item);
            IEnumerable<TodoItem> GetAll();
            TodoItem Find(long key);
            void Remove(long key);
            void Update(TodoItem item);
        }

    Within the Models folder, add a new file named TodoRepository.cs.

        using System;
        using System.Collections.Generic;
        using System.Linq;

        namespace TodoApi.Models;

        public class TodoRepository : ITodoRepository
        {
            private readonly TodoContext _context;
            public TodoRepository(TodoContext context)
            {
                _context = context;
                Add(new TodoItem { Name = "Item1"});
            }

            public IEnumerable<TodoItem> GetAll()
            {
                return _context.TodoItems.ToList();
            }
            public void Add(TodoItem item)
            {
                _context.TodoItems.Add(item);
                _context.SaveChanges();
            }
            public TodoItem Find(long key)
            {
                return _context.TodoItems.FirstOrDefault(t => t.Key == key);
            }
            public void Remove(long key)
            {
                var entity = _context.TodoItems.First(t => t.Key == key);
                _context.TodoItems.Remove(entity);
                _context.SaveChanges();
            }
            public void Update(TodoItem item)
            {
                _context.TodoItems.Update(item);
                _context.SaveChanges();
            }
        }

4. Register the database context and repository in Program.cs

        using TodoApi.Models;
        using Microsoft.EntityFrameworkCore;

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // register database context and repository
        builder.Services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase());
        builder.Services.AddSingleton<ITodoRepository, TodoRepository>;

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

5. Add a controller. In the Controllers folder, add a new TodoController.cs file.

        using System.Collections.Generic;
        using Microsoft.AspNetCore.Mvc;
        using TodoApi.Models;

        namespace TodoApi.Controllers;

        [Route("api/[controller]")]
        public class TodoController : ControllerBase
        {
            public ITodoRepository TodoItems { get; set; }
            public TodoController(ITodoRepository todoItems)
            {
                TodoItems = todoItems;
            }

            [HttpGet]
            public IEnumerable<TodoItem> GetAll()
            {
                return TodoItems.GetAll();
            }

            [HttpGet("{id}", Name = "GetTodo")]
            public IActionResult GetById(long id)
            {
                var item = TodoItems.Find(id);
                if (item == null)
                    return NotFound();

                return new ObjectResult(item);
            }

            [HttpPost]
            public IActionResult Create([FromBody] TodoItem item)
            {
                if (item == null)
                    return BadRequest();

                TodoItems.Add(item);
                return CreatedAtRoute("GetTodo", new { id= item.Key }, item);
            }

            [HttpPut("{id}")]
            public IActionResult Update(long id, [FromBody] TodoItem item)
            {
                if (item == null || item.Key != id)
                {
                    return BadRequest();
                }
                var todo = TodoItems.Find(id);

                if (todo == null)
                {
                    return NotFound();
                }

                todo.Name = item.Name;
                todo.IsComplete = item.IsComplete;
                TodoItems.Update(todo);
                return new NoContentResult();
            }

            [HttpDelete("{id}")]
            public IActionResult Delete(long id)
            {
                var todo = TodoItems.Find(id);

                if (todo == null)
                {
                    return NotFound();
                }

                TodoItems.Remove(id);
                return new NoContentResult();
            }
        }

## Excercise: Create a WebAPi

* The solution must be uploaded to a repository.
* Create a WebAPi with the methods:
* GET verb, HealthCheck resource, always returns 200 OK, no text. **get with the HealthCheck name that returns Ok()**
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


NameSpaces comúnmente en los proyectos:
CLIENT.System.Project.Layers

In our case:

PRFTLatam. Workshop.sln

***Proyects***
PRFTLatam. Workshop.WebAPI

PRFTLatam. Workshop.Services.Logic
PRFTLatam. Workshop.Services.Models
PRFTLatam. Workshop.Infrastructure.Files
PRFTLatam. Workshop.Services.Logic.Test

![Alt text](media/hexagonalArchitecture.png)

* PRFTLatam. Workshop.Infrastructure: 
    - Create interface (IFileParsing) for the method that will return all the ids from the csv file
    
* PRFTLatam. Workshop.Services:
    - Create implentation (FileParsing) of the interface (IFileParsing) that returns the ids from the csv file https://stackoverflow.com/questions/1405038/reading-a-csv-file-in-net 
    - Create interface (IIdentityService) for the id validation method
    - Create implementation (IdentityService) for that interface
    - Reference PRFTLatam. Workshop.Infrastructure

*  PRFTLatam. Workshop.WebAPI:
    - Create controller (IdentityController) to get the list of errors from the validation method in services
    - Reference PRFTLatam. Workshop.Services

    - On Program.cs make dependency injection with both interfaces and implementations


### My solution

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

# Week 4

## Idispose, Garbage Collector, Heap vs Stack, Managed vs Unmanaged Code

If destiny leads us to work on IoT, .NET on Arduino for example, it is essential, due to the limited amount of memory.

If we move as technical leaders and we need to do a cloud deployment in containers or functions, the cost and performance of our solution will require understanding memory consumption.

If we are working on a “traditional” backend and we use access to files or web services and we do a bad implementation, eventually the site will crash due to lack of memory; normally this problem is called “Memory Leak” (This should be detected by an Endurance Test).

### .NET memory and garbage collector

* .NET takes care of creating and destroying objects. Creates objects onto managed memory blocks (heaps) and destroys objects no longer in use
* When an application is executed, the .NET execution engine allocates two chunks of memory:
    * **Small Object Heap (SOH) for Objects < 85K**: It's a contiguos heap, objects are allocated secuentially and .NET maintains a pointer called the next object pointer that indicates where the next object in the sequence should be allocated. .NET also keeps track of objects that are no longer used.
        * If an object is in use by the application, there will be references to it on the stack, from global objects, static and even within the CPU registers.
        * Objects that don't have a reference can never be accessed and are of no further use to the application and they'll be garbage collected at some point

        There is a **stack** which holds object references created during function calls.
        When an object is created, it's allocated onto the heap at the location pointed to by the next object pointer, a reference is placed on the stack and the next object pointer is incremented by the size of the object.

        Using this mechanism, .net maintains a heap of consecutive objects

    * **Large Object Heap (LOH) for objects >= 85k**

* Whenever the `new` keyword is used in the code, the object is allocated into one of the heaps based on its size 
* The garbage collector runs whenever the memory used by the heap reaches certain thresholds. It identifies all the rooted objects which are the ones with an actual reference and then compacts the heap by copying the routed objects over the rootless ones, then the heap is compacted and the memory from the roptless object is reclaimed and the resets the pointer. 
* When the garbage collector runs all executing threads are suspended

### [.NET garbage collector interview questions](https://youtu.be/LplS0xp7VvU)

There are two types of memories
* Stack (4MB): It calls value types like int, float, bool.
* Heap (Unlimited): It calls reference types like obj, array, string. It's divided into:
    * Small Object Heap: Contains anything < 85K
    * Large Object Heap: Contains anything >= 85k


### [Back To Basics - Dispose Vs Finalize](https://www.c-sharpcorner.com/UploadFile/nityaprakash/back-to-basics-dispose-vs-finalize/)

#### Dispose
 
Garbage collector (GC) plays the main and important role in .NET for memory management so programmer can focus on the application functionality. Garbage collector is responsible for releasing the memory (objects) that is not being used by the application. 

To implement Dispose method for your custom class, you need to implement IDisposable interface. IDisposable interface expose Dispose method where code to release unmanaged resource will be written.

* It is used to dispose of unmanaged code such as files, database connections. This is called manually by the users own code. Use the dispose method when you are writing a custom class that will be used by other users

* It's a way to make sure unmanaged resources are cleaned up

##### Close Vs Dispose
 
Some objects expose Close and Dispose two methods. For Stream classes both serve the same purpose. Dispose method calls Close method inside.

#### Finalize

Finalize method also called destructor to the class. Finalize method can not be called explicitly in the code. Only the Garbage collector can call the the Finalize when an object becomes inaccessible. 
It is recommend that you implement the Finalize and Dispose methods together if you need to implement the Finalize method. After compilation, the destructor becomes the Finalize method.

Finalize is a bit expensive to use. It doesn't clean the memory immediately. When an application runs, the Garbage collector maintains a separate queue/array when it adds all objects which have finalized implemented. When the object is ready to claim memory, the Garbage Collector calls finalize method for that object and remove it from the collection. In this process it just cleans the memory that is used by unmanaged resource. Memory used by managed resource is still in heap as inaccessible reference. That memory releases whenever Garbage Collector run next time. Due to the finalize method GC will not clear the entire memory associated with an object in the first attempt.

* **It is always recommended that, one should not implement the Finalize method until it is extremely necessary. First priority should always be to implement the Dispose method and clean unmanaged as soon as possible when processing finish with that.**

### [The concept and the code of IDisposable](https://youtu.be/9ivQZ9LC7oc)

## Exercise: Data​​​​​​​​​​​​​

1. Finishing this part, we will have a .NET Junior/Intermediate Backend developer !!!!!!

    a. Mount a database on Azure SQL Server. (https://github.com/Jucer74/SQLCloud)
    * Download SQL Server Express
        ![sql-express-info](media/sql-express-info.png)
        `Server=localhost\SQLEXPRESS01;Database=master;Trusted_Connection=True;`
    * Download SQL Server Management Studio
    * Add the following to the appsettings.json file located in the webapi project:
    ``` json
    "ConnectionStrings": {
        "SQLServerConnectionStr":   "Server=localhost\\SQLEXPRESS01;Database=master;Trusted_Connection=True;"
    }
    ```


    b. Create a database model that supports this requirement.

    The order system:

    * A customer can place orders.
    * A client (Client):
    * A customer has an ID, text or numbers, maximum 32 characters. ***I'm assuming it's an integer***
    * A customer has a name, maximum 50 characters.
    * A client has a quota. In dollars. Maximum expected value 1,000,000. Something like 0.32 or 120.55 is valid

    A product (Product):

    * A product has a code, maximum 8 numerical characters.
    * A product has a name, maximum 20 alphanumeric characters.
    * A product has a price, maximum 5 figures, it can have two decimal places.

    An order (Order):

    * Order Number: A long.
    * Client ID.
    * Product ID.
    * Required quantity. Maximum 200.
    * Price at which the product was purchased when the order was placed.
    

    You can seed the data on your database by implementing Seed data approaches.

    * [Data Seeding](https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding)
    * [Migrations and seed (CodeMaze)](https://code-maze.com/migrations-and-seed-data-efcore/)


    If you want to generate Fake data, you can use [Bogus​​​​​​​](https://github.com/bchavez/Bogus) package.

2. Map the database to your solution in a new project called OrdersData (applying the naming structure already defined)

    Use the Entity Framework using the repository pattern.

    https://www.programmingwithwolfgang.com/repository-pattern-net-core/

    Include DataDecorators in Models.

    You don't have to implement the OnModelCreating() method, that's what we'll discuss in the review exercise.

    **Project creation**
    ``` shell
    mkdir PRFTLatam.OrdersData
    cd .\PRFTLatam.OrdersData\
    dotnet new sln -o PRFTLatam.OrdersData
    dotnet new webapi -o PRFTLatam.OrdersData.WebAPI
    dotnet new classlib -o PRFTLatam.OrdersData.Services
    dotnet new classlib -o PRFTLatam.OrdersData.Infrastructure
    dotnet sln add PRFTLatam.OrdersData.WebAPI/
    dotnet sln add PRFTLatam.OrdersData.Services/
    dotnet sln add PRFTLatam.OrdersData.Infrastructure/
    dotnet build


    Build succeeded.
    0 Warning(s)
    0 Error(s)


    dotnet add .\PRFTLatam.OrdersData.Services\ reference .\PRFTLatam.OrdersData.Infrastructure\

    dotnet add .\PRFTLatam.OrdersData.WebAPI\ reference .\PRFTLatam.OrdersData.Services\
    ```

    * Add EntityFrameworkCore related packages to Infrastructure:

    ``` shell
    dotnet add package Microsoft.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Tools 
    ```

    * Add more references in order to put the Dependency Injection in the Program.cs from the API, at the root of the project:

    ``` shell
    dotnet add .\PRFTLatam.OrdersData.WebAPI\ reference .\PRFTLatam.OrdersData.Infrastructure\
    ```

    * Add packages to the WebAPI:
    ``` shell
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
    dotnet add package Microsoft.EntityFrameworkCore.Design
    ```

    * Create migrations to add the database and update it [Entity Framework Core tools reference - .NET Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet), [Migrations Overview](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli):

    ``` shell
    cd .\PRFTLatam.OrdersData.WebAPI\
    dotnet ef migrations add InitialMigration
    dotnet ef database update
    ```

    Then in SQL Server Express it shows it like:
    ![Database](media/database.png)

    And also add the following to appsettings.json in the connection string:

    `TrustServerCertificate=True`


3. Implement Controllers and Services that allow publishing the following Rest services:

    Controller -> Service -> Repo (Why doesn't the Controller call the repo directly?)

    * GET api/v1/customers Shows all customers. paginated?

    * GET api/v1/products Show all products. paginated?

    * GET api/v1/orders Show all orders. paginated?

    * GET api/v1/customers/{id}/orders Shows all the orders of a customer. paginated?

    * GET api/v1/getAllCustomersWithNoOrder Returns all customers who have never had an order.

    * -POST api/v1/customer Allows you to create a customer

    Note correct HTTP return codes. If there is an error, return a list with the description of the errors.

4. Implement a unit test in the repository for one of the methods that allows you to do the GET.

    Implement unit test for the POST method.

    * [Unit tests in Entity Framework Core 5](https://www.michalbialecki.com/2020/11/28/unit-tests-in-entity-framework-core-5/)

    * Create the project for the tests
    ``` shell
    cd PRFTLatam.OrdersData
    dotnet new xunit -o PRFTLatam.OrdersData.Tests
    dotnet sln add PRFTLatam.OrdersData.Tests/
    dotnet add PRFTLatam.OrdersData.Tests/ reference PRFTLatam.OrdersData.Services/
    <!-- dotnet add PRFTLatam.OrdersData.Tests/ reference PRFTLatam.OrdersData.WebAPI/
    dotnet add PRFTLatam.OrdersData.Tests/ reference PRFTLatam.OrdersData.Infrastructure/ -->

    cd .\PRFTLatam.OrdersData.Tests\
    dotnet add package NSubstitute
    dotnet add package NSubstitute.Analyzers.CSharp

    ```

    [Choosing a testing strategy](https://learn.microsoft.com/en-us/ef/core/testing/choosing-a-testing-strategy)

    [Testing without your production database system](https://learn.microsoft.com/en-us/ef/core/testing/testing-without-the-database)

## [The Repository and Unit of Work Patterns](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)

The repository and unit of work patterns are intended to create an abstraction layer between the data access layer and the business logic layer of an application. Implementing these patterns can help insulate your application from changes in the data store and can facilitate automated unit testing or test-driven development (TDD).

In this tutorial you'll implement a repository class for each entity type. For the Student entity type you'll create a repository interface and a repository class. **When you instantiate the repository in your controller, you'll use the interface so that the controller will accept a reference to any object that implements the repository interface. When the controller runs under a web server, it receives a repository that works with the Entity Framework.** When the controller runs under a unit test class, it receives a repository that works with data stored in a way that you can easily manipulate for testing, such as an in-memory collection.

Later in the tutorial you'll use multiple repositories and a unit of work class for the Course and Department entity types in the Course controller. **The unit of work class coordinates the work of multiple repositories by creating a single database context class shared by all of them.** If you wanted to be able to perform automated unit testing, you'd create and use interfaces for these classes in the same way you did for the Student repository. 

The following illustration shows one way to conceptualize the relationships between the controller and context classes compared to not using the repository or unit of work pattern at all.

<img src="https://asp.net/media/2578149/Windows-Live-Writer_8c4963ba1fa3_CE3B_Repository_pattern_diagram_1df790d3-bdf2-4c11-9098-946ddd9cd884.png" width=700px/>

### Implement a Generic Repository and a Unit of Work Class

Creating a repository class for each entity type could result in a lot of redundant code, and it could result in partial updates. For example, suppose you have to update two different entity types as part of the same transaction. If each uses a separate database context instance, one might succeed and the other might fail. **One way to minimize redundant code is to use a generic repository, and one way to ensure that all repositories use the same database context (and thus coordinate all updates) is to use a unit of work class.**


* **The unit of work class serves one purpose: to make sure that when you use multiple repositories, they share a single database context. That way, when a unit of work is complete you can call the SaveChanges method on that instance of the context and be assured that all related changes will be coordinated. All that the class needs is a Save method and a property for each repository. Each repository property returns a repository instance that has been instantiated using the same database context instance as the other repository instances.**

### [Structure types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct)

### [Working with entity states](https://learn.microsoft.com/en-us/ef/ef6/saving/change-tracking/entity-state)

### [virtual keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual)

The virtual keyword is used to modify a method, property, indexer, or event declaration and allow for it to be overridden in a derived class. For example, this method can be overridden by any class that inherits it

An entity can be in one of five states as defined by the EntityState enumeration. These states are:

* **Added:** the entity is being tracked by the context but does not yet exist in the database
* **Unchanged:** the entity is being tracked by the context and exists in the database, and its property values have not changed from the values in the database
* **Modified:** the entity is being tracked by the context and exists in the database, and some or all of its property values have been modified
* **Deleted:** the entity is being tracked by the context and exists in the database, but has been marked for deletion from the database the next time SaveChanges is called
* **Detached:** the entity is not being tracked by the context

SaveChanges does different things for entities in different states:

* Unchanged entities are not touched by SaveChanges. Updates are not sent to the database for entities in the Unchanged state.
* Added entities are inserted into the database and then become Unchanged when SaveChanges returns.
* Modified entities are updated in the database and then become Unchanged when SaveChanges returns.
* Deleted entities are deleted from the database and are then detached from the context.

# Week 5

## Concepts

### [What are generics?](https://youtu.be/7VlykMssZzk) 

It allows to write classes and functions in a way that we don't specifically define a particular type. Then when the class or function is used, we pass what type we want to work with

Example: List<T>

Example with a method:

        private T[] CreateArray<T>(T firstElement, T secondElement)
        {
            return new T[] { firstElement, secondElement };
        }

And now to use it:

        int[] intArray = CreateArray<int>(5, 6);
        string[] stringArray = CreateArray<string>("asda", "asgte");

> T could be replaced by anything you want

### [What is an extension method?](https://youtu.be/DTTk2TVRk6E)

Allow you to add extra methods without modifying the original type

#### Static class
Useful container of methods that just operate on input parameters, you don't have to set any fields or properties

### [Uses of the using statement](https://youtu.be/n6XKyJUPHC8)

* Dispose an object
* Create aliases or import types in a namespace: `using <alias> = <namespace to import>`
* Import methods of a static class: `using static System.Math` -> now you can use any method in the static class Math and don't have to do Math.Abs() for example

### [What is reflection](https://youtu.be/nx-GGQyZ2cU)

Reflection allows you to inspect class metadata and even run methods dynamically

#### D-SIMPLE
Dynamically
See and
Invoke
Members
Properties
Loading
Examination

### [What are static classes and static methods?](https://youtu.be/1Gsh87GGeqc)

* Static class: Class that can't be instantiated
* Static method: Exists on the type and not the object 

Rules:
* You can't access anything non static unless you pass it in 
* Static methods can be overloaded but not overwritten 
* Everything inside of a static class has to ve static
* You can't inherit from a static class, static classes are by default sealed.
* Can't put an extension method on a static class

Use of static classes: Process something by passing it into a method, do some work on it and passing the result back again. You can't instantiate it but you can use all the methods from it
They allow you to perform operations without the overhead of instantiation

### [What are lambda expressions?](https://youtu.be/UuCe9uwNAqQ)

Se puede representar con la palabra reservada Func

Función anónima: Función que tiene un proceso interno que no tiene nombre

Ejemplo:

        Func<int, int> b = (a) => a*2;

* `Func` define el tipo que va a ser la función b
* `<int, int>` indica que el tipo de entrada y de salida es int
* `b` es el nombre de la variable que será la expresión lambda
* `(a)` es el parámetro de entrada
* `=>` es lo que la hace una función lamda, el inicio de ella
* `a * 2` es lo que hace la función con el parámetro de entrada, automáticamente se hace un return

Para ejecutar:

        int result = b(4);

* Si se quiere poner otro parámetro de entrada:

        Func<int, int, int> suma = (a, b) => a + b;
        int result = suma(4, 10);

    Se puede hacer una sentencia lambda, se tiene más de una línea de código:

        Func<int, int, int> suma = (a, b) =>
        {
            if (a > b) return a;
            else return b;
        };

    Y acá si se debe poner la palabra return. Se puede hacer con un operador ternario btw


* Función que regresa una evaluación que sea true o false cuando un número es par

    ``` csharp
    using System.Linq;
    using System.Collections.Generic;

    var numbers = new Lis<int> {3, 5, 7, 4, 8, 9, 2};
    Func<int, bool> GetEven = (number) => number % 2 == 0; // <- aquí la funcionalidad va a estar encapsulada y se puedreutilizar

    var evens = numbers.Where(GetEven);
    ```

    Esto permite usar opciones de filtrado de Linq, con el `Where` lo que hace es lo que devuelva true es lo que va a retornar

* Action -> No devuelve nada

    ``` csharp
    Action<int> print = (number) => Console.WriteLine(number);
    print(5);
    ```

* Funciones que reciben funciones, recibe un int y una función, devueve un enterp:

    ``` csharp
    Func<int, Func<int, int>, int> FnHigherOrder = (number, fn) => 
    {
        if (number > 100)
            return fn(number);
        else 
            return number;
    };

    var result = FnHigherOrder(600, n => n * 2); // mandando la función lambda tal cual
    ```

## Exercise

* Create a method in the repository that returns the Total Value of Orders per customer.

    Customer1 $10000
    Client2 $1000
    Client3 $2000

    Sorted alphabetically by customer name.

    Create a method in the repository that returns the Total Value of Orders per customer, for customers who have no balance.   Ordered from highest to lowest, according to the customer who has the most value in orders.

* Create the method to Create an ORDER. This must modify 2 tables:
    * On one hand, it must affect the value of the Client's credit, subtracting the value of the order.
    * On the other hand, you must create the order itself.

    If there are more than 10 orders for a given date (same day), the order cannot be registered.

    Try to write an extension method for the command: ToDate() that returns the date in ISO format YYYYMMDD.​​​​​​

In order to create the order, enter something like the following:

```json
{
  "clientId": "1",
  "productId": 1,
  "requiredQuantity": 3,
  "price": 0
}
```

Which returns the rest of the fields full:

```json
{
  "id": 16,
  "date": "2023-09-05T17:23:30.4273283-05:00",
  "clientId": "1",
  "productId": 1,
  "product": {
    "id": 1,
    "name": "Plant Based Meat",
    "price": 20000
  },
  "requiredQuantity": 3,
  "price": 60000
}
```

And then when getting the clients that are in the DB it shows the following:

```json
[
  {
    "id": "1",
    "name": "Pedro",
    "quota": 140000,
    "orders": [
      {
        "id": 16,
        "date": "2023-09-05T17:23:30.4273283",
        "clientId": "1",
        "productId": 1,
        "product": {
          "id": 1,
          "name": "Plant Based Meat",
          "price": 20000
        },
        "requiredQuantity": 3,
        "price": 60000
      }
    ],
    "ordersTotal": 60000
  },
  {
    "id": "2",
    "name": "Laura",
    "quota": 500000,
    "orders": [],
    "ordersTotal": 0
  },
  {
    "id": "4",
    "name": "Felipe",
    "quota": 950000,
    "orders": [
      {
        "id": 14,
        "date": "2023-09-05T17:21:30.2262284",
        "clientId": "4",
        "productId": 2,
        "product": {
          "id": 2,
          "name": "Soap",
          "price": 5000
        },
        "requiredQuantity": 2,
        "price": 10000
      },
      {
        "id": 15,
        "date": "2023-09-05T17:22:05.4536307",
        "clientId": "4",
        "productId": 1,
        "product": {
          "id": 1,
          "name": "Plant Based Meat",
          "price": 20000
        },
        "requiredQuantity": 2,
        "price": 40000
      }
    ],
    "ordersTotal": 50000
  }
]
   
```

# Code challenges

## 1. Keywords Usage

### Exercise 1
​​​​​​
Create a method that receives 3 variables as parameters: name, age and email then print this out in the following format.

```shell
name: John
age: 25
email: test@test.com 
```

### Exercise 
​​​​​​
Create a method that sums 1,…,n numbers as parameters and print the operation result.

```csharp
// Input arguments
2,4,5,8,10

// Output. A string that contains the operation result in this format
 2 + 4 + 5 + 8 + 10 = 29
```

### Exercise 
​​​​​​​
Create a number to word helper method that receives as a parameter (Read the salary from console redline) the salary of a developer. The requirements for the helper method are the following.

* Validates if the salary meets the criteria of being greater than 50.000 USD and return true or false in that case.

* If the salary meets the criteria in another variable, then returns the salary in letters “seventy-five thousand dollars”.

Number to word code snippet:

```csharp
string CurrencyToWord(int number, string word)
{
    if (number / 1_000_000 != 0)
    {
        word = string.Concat(CurrencyToWord(number / 1_000_000, word), " million ");
        number %= 1_000_000;
    }
​
    if (number / 1_000 != 0)
    {
        word = string.Concat(CurrencyToWord(number / 1_000, word), " thousand ");
        number %= 1_000;
    }
​
    if (number / 100 != 0)
    {
        word = string.Concat(CurrencyToWord(number / 100, word), " hundred ");
        number %= 100;
    }
​
    word = NumberToWord(number, word);
​
    return word;
}
​
string NumberToWord(int number, string words)
{
    if (words != "") words += " ";
​
    var unitValues = new[]
    {
        "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve",
        "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
    };
    var tensValues = new[]
        { "", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
​
​
    if (number >= 20)
    {
        words += tensValues[number / 10];
        if (number % 10 > 0) words += "-" + unitValues[number % 10];
    }
    else
        words += unitValues[number];
​
    return words;
}
​
​
// Usage
var phrase = CurrencyToWord(85000, string.Empty);
```


## 2. General

### Exercise 
​​​​​​
Create a method that calculates the salary of a developer. The method should receive as a parameter 2 variables: Salary base (Per hour) and the worked hours. One of the requirements is that the salary base might be an integer, decimal, float, or double value. The solution should cover all of those types.

#### Skills to evaluate
* Method overload
* Dynamic values

## 3. String operations

### Exercise 
​​​​​
Write a method that receive 4 names and concatenate it to print a full name with the following structure.

```
"Hello Tonny Anderson Stark Rogers"
```

Just to see how the different ways to concatenate strings work in C# create a method for each of the approaches mentioned on the official [doc](https://docs.microsoft.com/en-us/dotnet/csharp/how-to/concatenate-multiple-strings).

The idea here is to evaluate the performance of those separate methods by using [benchmark](https://benchmarkdotnet.org/articles/overview.html).

### Implementation

Install the package
```shell
dotnet add package BenchmarkDotNet --version 0.13.8
```

Create a new console application, write a class with methods that you want to measure, and mark them with the Benchmark attribute: `[Benchmark]`

The BenchmarkRunner.Run<Md5VsSha256>() call runs your benchmarks and prints results to the console.

Note that BenchmarkDotNet will only run benchmarks if the application is built in the Release configuration. This is to prevent unoptimized code from being benchmarked. BenchmarkDotNet will issue an error if you forget to change the configuration.

### [How to benchmark C# code using BenchmarkDotNet](https://www.infoworld.com/article/3573782/how-to-benchmark-csharp-code-using-benchmarkdotnet.html)

When benchmarking you should always ensure that you run your project in release mode. The reason is that during compilation the code is optimized differently for both debug and release modes. The C# compiler does a few optimizations in release mode that are not available in debug mode.

Hence you should run your project in the release mode only. To run benchmarking, specify the following command at the Visual Studio command prompt.

```shell
dotnet run -p PRFTLatam.StringOperations.csproj -c Release
```

#### Analyze the benchmarking results
Once the execution of the benchmarking process is complete, a summary of the results will be displayed at the console window. The summary section contains information related to the environment in which the benchmarks were executed, such as the BenchmarkDotNet version, operating system, computer hardware, .NET version, compiler information, and information related to the performance of the application.

A few files will also be created in the BenchmarkDotNet.Artifacts folder under the application’s root folder. Here is a summary of the results. 

```shell
BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3324/22H2/2022Update)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 6.0.413
  [Host]     : .NET 6.0.21 (6.0.2123.36311), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.21 (6.0.2123.36311), X64 RyuJIT AVX2


| Method                         | Mean            | Error         | StdDev        | Gen0   | Allocated |
|------------------------------- |----------------:|--------------:|--------------:|-------:|----------:|
| ConcatenateNames               | 1,279,975.38 ns | 13,291.035 ns | 11,098.616 ns |      - |    1769 B |
| ConcatenatePlusOperator        |        48.32 ns |      0.461 ns |      0.385 ns | 0.0140 |     176 B |
| ConcatenatePlusEqualOperator   |        36.39 ns |      0.127 ns |      0.106 ns | 0.0216 |     272 B |
| ConcatenateStringInterpolation |        39.97 ns |      0.807 ns |      0.897 ns | 0.0070 |      88 B |
| ConcatenateStringFormat        |        90.34 ns |      1.033 ns |      0.967 ns | 0.0114 |     144 B |
| ConcatenateStringBuilder       |        80.81 ns |      0.647 ns |      0.605 ns | 0.0331 |     416 B |
| ConcatenateStringConcat        |        31.93 ns |      0.163 ns |      0.153 ns | 0.0114 |     144 B |
| ConcatenateStringJoin          |        33.12 ns |      0.697 ns |      0.684 ns | 0.0121 |     152 B |
| ConcatenateLINQ                |        93.70 ns |      0.826 ns |      0.732 ns | 0.0299 |     376 B |
```

Adding the Rank column attribute and running the benchmarking again. This will add an extra column to the output indicating which method was faster:

```shell
BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3324/22H2/2022Update)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 6.0.413
  [Host]     : .NET 6.0.21 (6.0.2123.36311), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.21 (6.0.2123.36311), X64 RyuJIT AVX2


| Method                         | Mean     | Error    | StdDev   | Rank | Gen0   | Allocated |
|------------------------------- |---------:|---------:|---------:|-----:|-------:|----------:|
| ConcatenatePlusOperator        | 47.58 ns | 0.504 ns | 0.447 ns |    4 | 0.0140 |     176 B |
| ConcatenatePlusEqualOperator   | 37.23 ns | 0.351 ns | 0.293 ns |    2 | 0.0216 |     272 B |
| ConcatenateStringInterpolation | 39.83 ns | 0.237 ns | 0.210 ns |    3 | 0.0070 |      88 B |
| ConcatenateStringFormat        | 89.74 ns | 0.347 ns | 0.307 ns |    6 | 0.0114 |     144 B |
| ConcatenateStringBuilder       | 79.56 ns | 0.378 ns | 0.354 ns |    5 | 0.0331 |     416 B |
| ConcatenateStringConcat        | 31.51 ns | 0.068 ns | 0.061 ns |    1 | 0.0114 |     144 B |
| ConcatenateStringJoin          | 31.81 ns | 0.348 ns | 0.326 ns |    1 | 0.0121 |     152 B |
| ConcatenateLINQ                | 90.52 ns | 0.526 ns | 0.492 ns |    6 | 0.0299 |     376 B |
```

## 4. Collections

### Exercise

Create a console app that asks for a set of names and will end once the user types letter e then the app will print the names in the order they were introduced.

#### Requirements
Print the names in the order they were typed
Print the list of names inverted
Remove all the elements of the list once all the names are printed
At the same time, you are printing the names remove it from the list

#### Skills to evaluate
List operations (Add, Reverse, Clear, Remove)
Queue and Stack


### Material
* [Collections (C#)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/collections)


## 5. Developer Salary Calculator

### Exercise

Create a console app that reads a csv file that contains the information of a groups of developers classified by type. A developer might be classified in one of the following types:

* Junior
* Intermediate
* Senior
* Lead

Once you read the csv file it will be required to map each row in a defined type and then print all the information in the console in the following way.

Dev Name: John Doe
Dev Type: Junior
Worked Hours: 40
SalaryByHour: 120 USD

#### Increase Requirement 1
​​​​​
Instead of reading the data from a csv file you should read it from a simple table configure in a database​​​​​​​.

#### Increase Requirement 2
​​​​
Once you have all the developer data read done. You should create a method that calculates the salary of a list of developers and print the data in the console.

```csharp
// Salary Rules
BaseRule => Hours*SalaryByHour 
Junior => BaseRule 
Intermediate => BaseRule * 1.12 
Senior => BaseRule * 1.25 
Lead => BaseRule * 1.5 
​
//Format to print the data: 
Dev Name: John Doe 
Dev Type: Junior 
Worked Hours: 40 
SalaryByHour: 120 USD
​
Resume: 
Total Salary: 1500 USD 
Total Hours: 90 
Total Devs: 4
```

#### Skills to evaluate
​​​​​​
* File stream operations
* Model definition
* Value types (float, integers, string)
* Single Responsibility
* Polymorphism
* [Strategy Pattern](https://www.geeksforgeeks.org/strategy-pattern-set-1/)

### Implementation

Generate the data: https://generatedata.com/generator

## 6. Developer Todo

Create a console app that will receive as a parameter the developer user email e.g. Sincere@april.biz. Based on the entered user email get the user personal information and the user ToDos list by calling following services.

* https://jsonplaceholder.typicode.com/users
* https://jsonplaceholder.typicode.com/todos

Once you have all the user information (Personal and ToDos) it is necessary to write the result info in a json file in the following format.

```json
{
   "id":1,
   "name":"Leanne Graham",
   "username":"Bret",
   "email":"Sincere@april.biz",
   "address":{
      "street":"Kulas Light",
      "suite":"Apt. 556",
      "city":"Gwenborough",
      "zipcode":"92998-3874",
      "geo":{
         "lat":"-37.3159",
         "lng":"81.1496"
      }
   },
   "phone":"1-770-736-8031 x56442",
   "website":"hildegard.org",
   "todos":[
      {
         "userId":1,
         "id":1,
         "title":"delectus aut autem",
         "completed":false
      }
   ]
}
```

### Implementation

### Create console app

Use the following commands to do so:

```shell
mkdir PRFTLatam.DeveloperTodoClient
cd .\PRFTLatam.DeveloperTodoClient\
dotnet new console
dotnet add package Microsoft.AspNet.WebApi.Client --version 5.2.9 
dotnet add package Newtonsoft.Json --version 13.0.3
```

### Create the code in the Program.cs

```csharp
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace PRFTLatam.DeveloperTodoClient;

class Program
{
    static HttpClient client = new HttpClient();

    static void Main()
    {
        RunAsync().GetAwaiter().GetResult();
    }

    static async Task<JToken> GetDeveloperTodos(string email)
    {
        string userResponse = await client.GetStringAsync($"https://jsonplaceholder.typicode.com/users/?email={email}");
        JArray userArray = JArray.Parse(userResponse);
        JToken user = userArray[0];

        string userTodosResponse = await client.GetStringAsync($"https://jsonplaceholder.typicode.com/todos/?userId={user["id"]}");
        JArray userTodosArray = JArray.Parse(userTodosResponse);

        user["company"].Parent.Remove();
        
        user["todos"] = userTodosArray;

        return user;
    }

    static async Task RunAsync()
    {
        // var user = await GetDeveloperTodos("Sincere@april.biz");
        var user = await GetDeveloperTodos("Shanna@melissa.tv");
        Console.WriteLine(user);
        File.WriteAllText(@"C:\Users\laura.bustamanteh\Downloads\dotnet-ramp-up\PRFTLatam.DeveloperTodoClient\user.json", user.ToString());
    }
}
```

Output:

```json
{
  "id": 2,
  "name": "Ervin Howell",
  "username": "Antonette",
  "email": "Shanna@melissa.tv",
  "address": {
    "street": "Victor Plains",
    "suite": "Suite 879",
    "city": "Wisokyburgh",
    "zipcode": "90566-7771",
    "geo": {
      "lat": "-43.9509",
      "lng": "-34.4618"
    }
  },
  "phone": "010-692-6593 x09125",
  "website": "anastasia.net",
  "todos": [
    {
      "userId": 2,
      "id": 21,
      "title": "suscipit repellat esse quibusdam voluptatem incidunt",
      "completed": false
    },
    {
      "userId": 2,
      "id": 22,
      "title": "distinctio vitae autem nihil ut molestias quo",
      "completed": true
    },
    {
      "userId": 2,
      "id": 23,
      "title": "et itaque necessitatibus maxime molestiae qui quas velit",
      "completed": false
    },
    {
      "userId": 2,
      "id": 24,
      "title": "adipisci non ad dicta qui amet quaerat doloribus ea",
      "completed": false
    },
    {
      "userId": 2,
      "id": 25,
      "title": "voluptas quo tenetur perspiciatis explicabo natus",
      "completed": true
    },
    {
      "userId": 2,
      "id": 26,
      "title": "aliquam aut quasi",
      "completed": true
    },
    {
      "userId": 2,
      "id": 27,
      "title": "veritatis pariatur delectus",
      "completed": true
    },
    {
      "userId": 2,
      "id": 28,
      "title": "nesciunt totam sit blanditiis sit",
      "completed": false
    },
    {
      "userId": 2,
      "id": 29,
      "title": "laborum aut in quam",
      "completed": false
    },
    {
      "userId": 2,
      "id": 30,
      "title": "nemo perspiciatis repellat ut dolor libero commodi blanditiis omnis",
      "completed": true
    },
    {
      "userId": 2,
      "id": 31,
      "title": "repudiandae totam in est sint facere fuga",
      "completed": false
    },
    {
      "userId": 2,
      "id": 32,
      "title": "earum doloribus ea doloremque quis",
      "completed": false
    },
    {
      "userId": 2,
      "id": 33,
      "title": "sint sit aut vero",
      "completed": false
    },
    {
      "userId": 2,
      "id": 34,
      "title": "porro aut necessitatibus eaque distinctio",
      "completed": false
    },
    {
      "userId": 2,
      "id": 35,
      "title": "repellendus veritatis molestias dicta incidunt",
      "completed": true
    },
    {
      "userId": 2,
      "id": 36,
      "title": "excepturi deleniti adipisci voluptatem et neque optio illum ad",
      "completed": true
    },
    {
      "userId": 2,
      "id": 37,
      "title": "sunt cum tempora",
      "completed": false
    },
    {
      "userId": 2,
      "id": 38,
      "title": "totam quia non",
      "completed": false
    },
    {
      "userId": 2,
      "id": 39,
      "title": "doloremque quibusdam asperiores libero corrupti illum qui omnis",
      "completed": false
    },
    {
      "userId": 2,
      "id": 40,
      "title": "totam atque quo nesciunt",
      "completed": true
    }
  ]
}
```

### Material
* [​​​​​​​Calling a web api from a client](https://learn.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client)
* [Are you using HttpClient in the right way?](https://www.rahulpnath.com/blog/are-you-using-httpclient-in-the-right-way/)
* [Using HttpClientFactory](https://code-maze.com/using-httpclientfactory-in-asp-net-core-applications/)

### [​​​​​​​Calling a web api from a client](https://learn.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client)

This tutorial shows how to call a web API from a .NET application, using System.Net.Http.HttpClient.

In this tutorial, a client app is written that consumes the following web API:

<table aria-label="Table 1" class="table table-sm">
<thead>
<tr>
<th>Action</th>
<th>HTTP method</th>
<th>Relative URI</th>
</tr>
</thead>
<tbody>
<tr>
<td>Get a product by ID</td>
<td>GET</td>
<td>/api/products/<em>id</em></td>
</tr>
<tr>
<td>Create a new product</td>
<td>POST</td>
<td>/api/products</td>
</tr>
<tr>
<td>Update a product</td>
<td>PUT</td>
<td>/api/products/<em>id</em></td>
</tr>
<tr>
<td>Delete a product</td>
<td>DELETE</td>
<td>/api/products/<em>id</em></td>
</tr>
</tbody>
</table>

For simplicity, the client application in this tutorial is a Windows console application.

NOTE: If you pass base URLs and relative URIs as hard-coded values, be mindful of the rules for utilizing the HttpClient API. The HttpClient.BaseAddress property should be set to an address with a trailing forward slash (/). For example, when passing hard-coded resource URIs to the HttpClient.GetAsync method, don't include a leading forward slash. To get a Product by ID:

* Set `client.BaseAddress = new Uri("https://localhost:5001/");`
* Request a Product. For example, `client.GetAsync<Product>("api/products/4");`.

#### Create the Console Application
In Visual Studio, create a new Windows console app named HttpClientSample and paste in the following code:

```csharp
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientSample
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Name: {product.Name}\tPrice: " +
                $"{product.Price}\tCategory: {product.Category}");
        }

        static async Task<Uri> CreateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/products", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<Product> GetProductAsync(string path)
        {
            Product product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Product>();
            }
            return product;
        }

        static async Task<Product> UpdateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/products/{product.Id}", product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            product = await response.Content.ReadAsAsync<Product>();
            return product;
        }

        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/products/{id}");
            return response.StatusCode;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:64195/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                Product product = new Product
                {
                    Name = "Gizmo",
                    Price = 100,
                    Category = "Widgets"
                };

                var url = await CreateProductAsync(product);
                Console.WriteLine($"Created at {url}");

                // Get the product
                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                // Update the product
                Console.WriteLine("Updating price...");
                product.Price = 80;
                await UpdateProductAsync(product);

                // Get the updated product
                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                // Delete the product
                var statusCode = await DeleteProductAsync(product.Id);
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
```

The preceding code is the complete client app.

* `RunAsync` runs and blocks until it completes. Most HttpClient methods are async, because they perform network I/O. All of the async tasks are done inside RunAsync. Normally an app doesn't block the main thread, but this app doesn't allow any interaction.

* The `Product class` matches the data model used by the web API. An app can use HttpClient to read a Product instance from an HTTP response. The app doesn't have to write any deserialization code.

* The `Program class`:
    * Create and Initialize HttpClient: The static HttpClient property is intended to be instantiated once and reused throughout the life of an application. The following conditions can result in SocketException errors: Creating a new HttpClient instance per request, Server under heavy load. Creating a new HttpClient instance per request can exhaust the available sockets.

    * The following code initializes the HttpClient instance:
        ```csharp
        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:64195/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        ```
        The preceding code:

        - Sets the base URI for HTTP requests. Change the port number to the port used in the server app. The app won't work unless port for the server app is used.
        - Sets the Accept header to "application/json". Setting this header tells the server to send data in JSON format.

    * **Send a GET request to retrieve a resource:** The following code sends a GET request for a product:
        ```csharp
        static async Task<Product> GetProductAsync(string path)
        {
            Product product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Product>();
            }
            return product;
        }
        ```
        The `GetAsync` method sends the HTTP GET request. When the method completes, it returns an `HttpResponseMessage` that contains the HTTP response. If the status code in the response is a success code, the response body contains the JSON representation of a product. Call `ReadAsAsync` to deserialize the JSON payload to a Product instance. The ReadAsAsync method is asynchronous because the response body can be arbitrarily large.

        HttpClient does not throw an exception when the HTTP response contains an error code. Instead, the IsSuccessStatusCode property is false if the status is an error code.   

    * **Sending a POST Request to Create a Resource:** The following code sends a POST request that contains a Product instance in JSON format: 

        ```csharp
        static async Task<Uri> CreateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/products", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }
        ```

        The `PostAsJsonAsync` method:

        * Serializes an object to JSON.
        * Sends the JSON payload in a POST request.
        
        If the request succeeds:

        * It should return a 201 (Created) response.
        * The response should include the URL of the created resources in the Location header.

    * **Sending a PUT Request to Update a Resource:** The following code sends a PUT request to update a product:
        ```csharp
        static async Task<Product> UpdateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/products/{product.Id}", product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            product = await response.Content.ReadAsAsync<Product>();
            return product;
        }
        ```

        The `PutAsJsonAsync` method works like PostAsJsonAsync, except that it sends a PUT request instead of POST.

    * **Sending a DELETE Request to Delete a Resource:** The following code sends a DELETE request to delete a product:
        ```csharp
        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/products/{id}");
            return response.StatusCode;
        }
        ```

        Like GET, a DELETE request does not have a request body. You don't need to specify JSON or XML format with DELETE.

#### Install the Web API Client Libraries

Use NuGet Package Manager to install the Web API Client Libraries package. https://www.nuget.org/packages/Microsoft.AspNet.WebApi.Client

```shell
dotnet add package Microsoft.AspNet.WebApi.Client --version 5.2.9
```

The preceding command adds the following NuGet packages to the project:

* Microsoft.AspNet.WebApi.Client
* Newtonsoft.Json

Newtonsoft.Json (also known as Json.NET) is a popular high-performance JSON framework for .NET.

### [Are you using HttpClient in the right way?](https://www.rahulpnath.com/blog/are-you-using-httpclient-in-the-right-way/)

When using ASP.NET to build an application, HTTP requests is made using an instance of the HttpClient class. An HttpClient class acts as a session to send HTTP Requests. It is a collection of settings applied to all requests executed by that instance.




## 7. Web API

### Exercise 
​​​​​
Create a web API that manages employment information and calculates the salary of the developers who work for it.

In order to store the developer information, it will be necessary to design a database model that satisfies the following criteria.

Developer attributes:

* First name
* Last name
* Full name -> The concatenation of the First name + Last name
* Age
* Worked hours
* Salary by hours
* Developer type
* Email

A developer might be classified in one of the following types:

* Junior
* Intermediate
* Senior
* Lead

The requirements for the web API are basically exposes a swagger page to access each of the methods the API exposes.
The solution should expose the capability of

* Create a developer: This functionality should receive and validate as a parameter the developers attributes such as:
    * First name: 3 characters of minimum length and 20 characters of max
    * Last name: 3 characters of minimum length and 30 characters of max
    * Age: Greater than 10 and numeric
    * Worked hours: Greater than 30 and less than 50
    * Salary by hours: Greater than 13
    * Developer type: Must be one of the accepted dev type Junior, Intermediate, Senior, and Lead
* Get the developer list information
* Search a developer by criteria: First name, Last name, Age, Worked Hours (Return a list of devs that met the criteria)
* Get developer by email (Return dev object is found and null/empty if not)
* Delete a developer by passing the email
* Update the developer information: For this the payload can be the same passed in the create developer and the validations should be the same

### Implentation

#### Create project

```shell
mkdir PRFTLatam.EmploymentInfo
cd .\PRFTLatam.EmploymentInfo\
dotnet new sln -n PRFTLatam.EmploymentInfo
dotnet new webapi -o PRFTLatam.EmploymentInfo.Infrastructure
dotnet new classlib -o PRFTLatam.EmploymentInfo.Application
dotnet new classlib -o PRFTLatam.EmploymentInfo.Domain
dotnet sln add PRFTLatam.EmploymentInfo.Infrastructure/
dotnet sln add PRFTLatam.EmploymentInfo.Application/
dotnet sln add PRFTLatam.EmploymentInfo.Domain/
dotnet build
```

<img src="https://miro.medium.com/max/1104/1*VhTqNKaR2gBsWXefCEV7Ig.png" width=700px/>

#### Create generic repository, unit of work and context

Add EntityFrameworkCore related packages to Domain:

    ``` shell
    dotnet add package Microsoft.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Tools 
    ```

#### Create Developer model, service and controller

Add references to the different projects

```shell
dotnet add PRFTLatam.EmploymentInfo.Application/ reference PRFTLatam.EmploymentInfo.Domain/
dotnet add PRFTLatam.EmploymentInfo.Infrastructure/ reference PRFTLatam.EmploymentInfo.Application/
dotnet add PRFTLatam.EmploymentInfo.Infrastructure/ reference PRFTLatam.EmploymentInfo.Domain/
```

#### Migrations

In order to create the migrations use the following command:

```shell
dotnet ef migrations add InitialMigration --project PRFTLatam.EmploymentInfo.Domain -s PRFTLatam.EmploymentInfo.Infrastructure -c EmploymentInfoContext --verbose
```

This is the structure of the command:

````shell
dotnet ef migrations add InitDatabase --project YourDataAccessLibraryName -s YourWebProjectName -c YourDbContextClassName --verbose 
````


https://stackoverflow.com/questions/59796411/unable-to-create-an-object-of-type-applicationdbcontext-for-the-different-pat

The `--verbose` flag is to see all the process it does to perform the migration

* To remove the most recent migration use:

```shell
dotnet ef migrations remove --project PRFTLatam.EmploymentInfo.Domain -s PRFTLatam.EmploymentInfo.Infrastructure -c EmploymentInfoContext --verbose
```

And then use the following command to update the database:


```shell
dotnet ef database update InitialMigration --project PRFTLatam.EmploymentInfo.Domain -s PRFTLatam.EmploymentInfo.Infrastructure -c EmploymentInfoContext --verbose

```

In the end this is how the API ends up looking in swagger:

<img src="media\employmentinfo-api.png" width=700px />

### Material
* [RESTful web API design](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design)
* [Get started with Swashbuckle and ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio)
* [Generic Repository & Unit Of Work Patterns in .NET](https://tomasznowok.medium.com/generic-repository-unit-of-work-patterns-in-net-b830b7fb5668)
* [Repository Pattern in .NET Core](https://www.programmingwithwolfgang.com/repository-pattern-net-core/)
* ​​​​​​[Options pattern in ASP.NET Core](​https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0)

* [**StatusCodes Class**](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.statuscodes?view=aspnetcore-7.0)

### [RESTful web API design](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design)

#### Define API operations in terms of HTTP methods

The effect of a specific request should depend on whether the resource is a collection or an individual item. The following table summarizes the common conventions adopted by most RESTful implementations using the e-commerce example. Not all of these requests might be implemented—it depends on the specific scenario.

<table aria-label="Table 1" class="table table-sm">
<thead>
<tr>
<th><strong>Resource</strong></th>
<th><strong>POST</strong></th>
<th><strong>GET</strong></th>
<th><strong>PUT</strong></th>
<th><strong>DELETE</strong></th>
</tr>
</thead>
<tbody>
<tr>
<td>/customers</td>
<td>Create a new customer</td>
<td>Retrieve all customers</td>
<td>Bulk update of customers</td>
<td>Remove all customers</td>
</tr>
<tr>
<td>/customers/1</td>
<td>Error</td>
<td>Retrieve the details for customer 1</td>
<td>Update the details of customer 1 if it exists</td>
<td>Remove customer 1</td>
</tr>
<tr>
<td>/customers/1/orders</td>
<td>Create a new order for customer 1</td>
<td>Retrieve all orders for customer 1</td>
<td>Bulk update of orders for customer 1</td>
<td>Remove all orders for customer 1</td>
</tr>
</tbody>
</table>

#### GET methods
A successful GET method typically returns HTTP status code 200 (OK). If the resource cannot be found, the method should return 404 (Not Found).

If the request was fulfilled but there is no response body included in the HTTP response, then it should return HTTP status code 204 (No Content); for example, a search operation yielding no matches might be implemented with this behavior.

#### POST methods
If a POST method creates a new resource, it returns HTTP status code 201 (Created). The URI of the new resource is included in the Location header of the response. The response body contains a representation of the resource.

If the method does some processing but does not create a new resource, the method can return HTTP status code 200 and include the result of the operation in the response body. Alternatively, if there is no result to return, the method can return HTTP status code 204 (No Content) with no response body.

If the client puts invalid data into the request, the server should return HTTP status code 400 (Bad Request). The response body can contain additional information about the error or a link to a URI that provides more details.

#### PUT methods
If a PUT method creates a new resource, it returns HTTP status code 201 (Created), as with a POST method. If the method updates an existing resource, it returns either 200 (OK) or 204 (No Content). In some cases, it might not be possible to update an existing resource. In that case, consider returning HTTP status code 409 (Conflict).

Consider implementing bulk HTTP PUT operations that can batch updates to multiple resources in a collection. The PUT request should specify the URI of the collection, and the request body should specify the details of the resources to be modified. This approach can help to reduce chattiness and improve performance.

#### PATCH methods
With a PATCH request, the client sends a set of updates to an existing resource, in the form of a patch document. The server processes the patch document to perform the update. The patch document doesn't describe the whole resource, only a set of changes to apply. The specification for the PATCH method (RFC 5789) doesn't define a particular format for patch documents. The format must be inferred from the media type in the request.

JSON is probably the most common data format for web APIs. There are two main JSON-based patch formats, called JSON patch and JSON merge patch.

JSON merge patch is somewhat simpler. The patch document has the same structure as the original JSON resource, but includes just the subset of fields that should be changed or added. In addition, a field can be deleted by specifying null for the field value in the patch document. (That means merge patch is not suitable if the original resource can have explicit null values.)

For example, suppose the original resource has the following JSON representation:

```json
{
    "name":"gizmo",
    "category":"widgets",
    "color":"blue",
    "price":10
}
```
Here is a possible JSON merge patch for this resource:

```json
{
    "price":12,
    "color":null,
    "size":"small"
}
```
This tells the server to update price, delete color, and add size, while name and category are not modified. For the exact details of JSON merge patch, see RFC 7396. The media type for JSON merge patch is application/merge-patch+json.

Here are some typical error conditions that might be encountered when processing a PATCH request, along with the appropriate HTTP status code.

<table aria-label="Table 2" class="table table-sm">
<thead>
<tr>
<th>Error condition</th>
<th>HTTP status code</th>
</tr>
</thead>
<tbody>
<tr>
<td>The patch document format isn't supported.</td>
<td>415 (Unsupported Media Type)</td>
</tr>
<tr>
<td>Malformed patch document.</td>
<td>400 (Bad Request)</td>
</tr>
<tr>
<td>The patch document is valid, but the changes can't be applied to the resource in its current state.</td>
<td>409 (Conflict)</td>
</tr>
</tbody>
</table>

#### DELETE methods
If the delete operation is successful, the web server should respond with HTTP status code 204 (No Content), indicating that the process has been successfully handled, but that the response body contains no further information. If the resource doesn't exist, the web server can return HTTP 404 (Not Found).

#### Asynchronous operations
Sometimes a POST, PUT, PATCH, or DELETE operation might require processing that takes a while to complete. If you wait for completion before sending a response to the client, it may cause unacceptable latency. If so, consider making the operation asynchronous. Return HTTP status code 202 (Accepted) to indicate the request was accepted for processing but is not completed.

You should expose an endpoint that returns the status of an asynchronous request, so the client can monitor the status by polling the status endpoint.

#### Filter and paginate data
Exposing a collection of resources through a single URI can lead to applications fetching large amounts of data when only a subset of the information is required. For example, suppose a client application needs to find all orders with a cost over a specific value. It might retrieve all orders from the /orders URI and then filter these orders on the client side. Clearly this process is highly inefficient. It wastes network bandwidth and processing power on the server hosting the web API.

Instead, the API can allow passing a filter in the query string of the URI, such as /orders?minCost=n. The web API is then responsible for parsing and handling the minCost parameter in the query string and returning the filtered results on the server side.

GET requests over collection resources can potentially return a large number of items. You should design a web API to limit the amount of data returned by any single request. Consider supporting query strings that specify the maximum number of items to retrieve and a starting offset into the collection. For example:

`/orders?limit=25&offset=50`

Also consider imposing an upper limit on the number of items returned, to help prevent Denial of Service attacks. To assist client applications, GET requests that return paginated data should also include some form of metadata that indicate the total number of resources available in the collection.

You can use a similar strategy to sort data as it is fetched, by providing a sort parameter that takes a field name as the value, such as /orders?sort=ProductID. However, this approach can have a negative effect on caching, because query string parameters form part of the resource identifier used by many cache implementations as the key to cached data.

You can extend this approach to limit the fields returned for each item, if each item contains a large amount of data. For example, you could use a query string parameter that accepts a comma-delimited list of fields, such as /orders?fields=ProductID,Quantity.

Give all optional parameters in query strings meaningful defaults. For example, set the limit parameter to 10 and the offset parameter to 0 if you implement pagination, set the sort parameter to the key of the resource if you implement ordering, and set the fields parameter to all fields in the resource if you support projections.


### [Fluent API - Configuring and Mapping Properties and Types](https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties)

#### Property Mapping
The Property method is used to configure attributes for each property belonging to an entity or complex type. The Property method is used to obtain a configuration object for a given property. The options on the configuration object are specific to the type being configured; IsUnicode is available only on string properties for example.

To explicitly set a property to be a primary key, you can use the HasKey method. In the following example, the HasKey method is used to configure the InstructorID primary key on the OfficeAssignment type.

```csharp
modelBuilder.Entity<OfficeAssignment>().HasKey(t => t.InstructorID);
```
* **Switching off Identity for Numeric Primary Keys**

The following example sets the DepartmentID property to System.ComponentModel.DataAnnotations.DatabaseGeneratedOption.None to indicate that the value will not be generated by the database.

```csharp
modelBuilder.Entity<Department>().Property(t => t.DepartmentID)
    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
```

* **Specifying the Maximum Length on a Property**

In the following example, the Name property should be no longer than 50 characters. If you make the value longer than 50 characters, you will get a DbEntityValidationException exception. If Code First creates a database from this model it will also set the maximum length of the Name column to 50 characters.

```csharp
modelBuilder.Entity<Department>().Property(t => t.Name).HasMaxLength(50);
```
* **Mapping an Entity Type to a Specific Table in the Database**

All properties of Department will be mapped to columns in a table called t_ Department.

```csharp
modelBuilder.Entity<Department>()  
    .ToTable("t_Department");
```

## 8. Background service

### Exercise
Create a console app that runs a background process to print the current time of the following cities each 30 seconds.

<table class="bandedRowColumnTableStyleTheme cke_show_border"><tbody><tr><td role="columnheader" style="text-align:center;">City</td><td role="columnheader" style="text-align:center;">TimeZone</td></tr><tr><td role="rowheader">Bogota</td><td>America/Bogota</td></tr><tr><td role="rowheader">Chicago</td><td>America/Chicago</td></tr><tr><td role="rowheader">Argentina</td><td>America/Argentina/Buenos_Aires</td></tr><tr><td role="rowheader">Detroit</td><td>America/Detroit</td></tr><tr><td role="rowheader">London</td><td>Europe/London</td></tr></tbody></table>

This information should be stored as a private static [read only collection](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlycollection-1?view=net-6.0).

Once you read the information you should print the information in the following format.

```
City: Bogota 
TimeZone: America/Bogota 
Time:2022-05-06T14:29:00.5-05:00
​
City: Chicago 
TimeZone: America/Chicago 
Time: 2022-05-06T14:29:00.5-05:00
​
City: Argentina 
TimeZone: America/Argentina/Buenos_Aires 
Time: 2022-05-06T16:29:00.5-05:00
​
City: Detroit 
TimeZone: America/Detroit 
Time: 2022-05-06T15:29:00.5-05:00
​
City: London 
TimeZone: Europe/London 
Time: 2022-05-06T20:29:00.5-05:00
```

TimeZone Package: [repo](https://github.com/mattjohnsonpint/TimeZoneConverter)
TimeZone Country List: [link](https://en.wikipedia.org/wiki/List_of_tz_database_time_zones)
TimeStringFormat: yyyy-MM-dd'T'HH:mm:ss.FFFzzz

### Increase Requirement 1
​​
Encapsulate the time converter implementation in a single class (TimeService/TimeProvider) and inject it in the hosted/background service as Transient, Singleton and then Scoped.

Note: For this one Scoped the idea is to see how this behaves in a hosted service as it is not the same for the others. See more info [here](https://docs.microsoft.com/en-us/dotnet/core/extensions/scoped-service).

#### Encapsulation: TimeService class

Reference: https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service?pivots=dotnet-6-0

```csharp
namespace TimeZonesBackgroundService;

public sealed class TimeService
{
    public string GetTime()
    {
        string bogota = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(0));
        string chicago = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(1));
        string argentina = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(2));
        string detroit = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(3));
        string london = GetTimeZoneDisplayed(timeZonesCollection.ElementAt(4));

        return $"{bogota}{chicago}{argentina}{detroit}{london}";
    }

    private string GetTimeZoneDisplayed(TimeZones timeZone)
    {
        return $"City: {timeZone.city}\nTimeZone: {timeZone.timeZoneName}\nTime: {timeZone.timeZoneTime.ToString("yyyy-MM-dd'T'HH:mm:ss.FFFzzz")}\n\n"; 
    }

    private static List<TimeZones> timeZonesList = new(){
        new TimeZones("Bogota", "America/Bogota", TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("America/Bogota"))),
        new TimeZones("Chicago", "America/Chicago", TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("America/Chicago"))),
        new TimeZones("Argentina", "America/Argentina/Buenos_Aires", TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("America/Argentina/Buenos_Aires"))),
        new TimeZones("Detroit", "America/Detroit", TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("America/Detroit"))),
        new TimeZones("London", "Europe/London", TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("Europe/London")))
    };
    private static ReadOnlyCollection<TimeZones> timeZonesCollection = new ReadOnlyCollection<TimeZones>(timeZonesList);
}

readonly record struct TimeZones(string city, string timeZoneName, DateTime timeZoneTime);
```

#### Injection as Singleton

```csharp
using TimeZonesBackgroundService;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = ".NET Time Zones Service";
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<TimeService>();
        // services.AddTransient<TimeService>();
        services.AddHostedService<WindowsBackgroundService>();
    })
    .Build();

await host.RunAsync();
```

#### Background service class updated

```csharp
using System;
using TimeZoneConverter;
using System.Collections.ObjectModel;

namespace TimeZonesBackgroundService;

public sealed class WindowsBackgroundService : BackgroundService
{
    private readonly ILogger<WindowsBackgroundService> _logger;
    private readonly TimeService _timeService;

    public WindowsBackgroundService(ILogger<WindowsBackgroundService> logger, TimeService timeService)
    {
        _timeService = timeService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string time = _timeService.GetTime();
                _logger.LogInformation("{time}", time);

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }   
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
            Environment.Exit(1);
        }
    }
}

```

#### Publish the service

```shell
dotnet publish -c Release -r win-x64 --self-contained true
```

#### Create the Windows Service

To create the Windows Service, use the native Windows Service Control Manager's (sc.exe) create command. Run PowerShell as an Administrator.

```powershell
sc.exe create ".NET Time Zones Service" binpath="C:\Users\laura.bustamanteh\Downloads\dotnet-ramp-up\TimeZonesBackgroundService\bin\Release\net6.0\win-x64\publish\WindowsBackgroundService.exe"
```

#### Start the Windows Service

```powershell
sc.exe start ".NET Time Zones Service"
```

#### View logs
Now every 30 seconds it shows the same result as before

<img src="media\timezones-background-service-event-viewer-result.png" width=700px/>


#### Stop the Windows Service

```powershell
sc.exe stop ".NET Time Zones Service"
```

#### Remove the windows service

````powershell
sc.exe delete ".NET Time Zones Service"
````

### Increase Requirement 2
​​
Read the info for the cities from a different source. Instead of having it as a static read only collection read it from a database.

### Increase Requirement 3
​​​​​
Display a message once the service is stopped:

```
"Background service completed"
```

#### Override the method in the windows background service class:

```csharp
public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(WindowsBackgroundService)} completed");

        await base.StopAsync(stoppingToken);
    }
```

The way it's shown:

<img src="media\timezones-background-service-requirement-3.png" width=700px/>


**​​​​​​​​​​Material**
​​​​​
* [Hosted services](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio)
* [Worker services in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/workers?pivots=dotnet-7-0)
* [Running .NET Core Applications as a Windows Service](https://code-maze.com/aspnetcore-running-applications-as-windows-service/)


### [Background tasks with hosted services in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio)

In ASP.NET Core, background tasks can be implemented as hosted services. A hosted service is a class with background task logic that implements the IHostedService interface.

Hosted service examples:

* Background task that runs on a timer.
* Hosted service that activates a [scoped service](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0#service-lifetimes). The scoped service can use dependency injection (DI).
* Queued background tasks that run sequentially.

#### Worker Service template
The ASP.NET Core Worker Service template provides a starting point for writing long running service apps. An app created from the Worker Service template specifies the Worker SDK in its project file:

```xml
<Project Sdk="Microsoft.NET.Sdk.Worker">
```

To use the template as a basis for a hosted services app:

Use the Worker Service (worker) template with the dotnet new command from a command shell. In the following example, a Worker Service app is created named ContosoWorker. A folder for the ContosoWorker app is created automatically when the command is executed.

```shell
dotnet new worker -o ContosoWorker
```

#### Package
An app based on the Worker Service template uses the Microsoft.NET.Sdk.Worker SDK and has an explicit package reference to the [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting) package. For example, see the sample app's project file (BackgroundTasksSample.csproj).


#### IHostedService interface
The IHostedService interface defines two methods for objects that are managed by the host. These two methods serve as lifecycle methods - they're called during host start and stop events respectively.:

* **[StartAsync(CancellationToken)](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostedservice.startasync)**: contains the logic to start the background task. StartAsync is called before:

    * The app's request processing pipeline is configured.
    * The server is started and [IApplicationLifetime.ApplicationStarted](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.iapplicationlifetime.applicationstarted) is triggered.

    StartAsync should be limited to short running tasks because hosted services are run sequentially, and no further services are started until StartAsync runs to completion.

* **[StopAsync(CancellationToken)](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostedservice.stopasync)**: is triggered when the host is performing a graceful shutdown. StopAsync contains the logic to end the background task. Implement IDisposable and finalizers (destructors) to dispose of any unmanaged resources.

> When overriding either StartAsync or StopAsync methods, you must call and await the base class method to ensure the service starts and/or shuts down properly.

The cancellation token has a default 30 second timeout to indicate that the shutdown process should no longer be graceful. When cancellation is requested on the token:

* Any remaining background operations that the app is performing should be aborted.
* Any methods called in StopAsync should return promptly.

However, tasks aren't abandoned after cancellation is requested—the caller awaits all tasks to complete.

If the app shuts down unexpectedly (for example, the app's process fails), StopAsync might not be called. Therefore, any methods called or operations conducted in StopAsync might not occur.

To extend the default 30 second shutdown timeout, set:

* [ShutdownTimeout](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostoptions.shutdowntimeout) when using Generic Host. For more information, see [.NET Generic Host in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-6.0#shutdowntimeout).
* Shutdown timeout host configuration setting when using Web Host. For more information, see [ASP.NET Core Web Host](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-6.0#shutdown-timeout).

The hosted service is activated once at app startup and gracefully shut down at app shutdown. If an error is thrown during background task execution, Dispose should be called even if StopAsync isn't called.

#### BackgroundService base class

BackgroundService is a base class for implementing a long running IHostedService.

ExecuteAsync(CancellationToken) is called to run the background service. The implementation returns a Task that represents the entire lifetime of the background service. No further services are started until ExecuteAsync becomes asynchronous, such as by calling await. Avoid performing long, blocking initialization work in ExecuteAsync. The host blocks in StopAsync(CancellationToken) waiting for ExecuteAsync to complete.

The cancellation token is triggered when IHostedService.StopAsync is called. Your implementation of ExecuteAsync should finish promptly when the cancellation token is fired in order to gracefully shut down the service. Otherwise, the service ungracefully shuts down at the shutdown timeout. For more information, see the IHostedService interface section.

#### Timed background tasks
A timed background task makes use of the System.Threading.Timer class. The timer triggers the task's DoWork method. The timer is disabled on StopAsync and disposed when the service container is disposed on Dispose:

```csharp
public class TimedHostedService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<TimedHostedService> _logger;
    private Timer? _timer = null;

    public TimedHostedService(ILogger<TimedHostedService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
```

The Timer doesn't wait for previous executions of DoWork to finish, so the approach shown might not be suitable for every scenario. Interlocked.Increment is used to increment the execution counter as an atomic operation, which ensures that multiple threads don't update executionCount concurrently.

The service is registered in IHostBuilder.ConfigureServices (Program.cs) with the AddHostedService extension method:

```csharp
services.AddHostedService<TimedHostedService>();
```

#### Consuming a scoped service in a background task
To use scoped services within a BackgroundService, create a scope. No scope is created for a hosted service by default.

The scoped background task service contains the background task's logic. In the following example:

* The service is asynchronous. The DoWork method returns a Task. For demonstration purposes, a delay of ten seconds is awaited in the DoWork method.
* An ILogger is injected into the service.

```csharp
internal interface IScopedProcessingService
{
    Task DoWork(CancellationToken stoppingToken);
}

internal class ScopedProcessingService : IScopedProcessingService
{
    private int executionCount = 0;
    private readonly ILogger _logger;
    
    public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
    {
        _logger = logger;
    }

    public async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            executionCount++;

            _logger.LogInformation(
                "Scoped Processing Service is working. Count: {Count}", executionCount);

            await Task.Delay(10000, stoppingToken);
        }
    }
}
```

The hosted service creates a scope to resolve the scoped background task service to call its DoWork method. DoWork returns a Task, which is awaited in ExecuteAsync:

```csharp
public class ConsumeScopedServiceHostedService : BackgroundService
{
    private readonly ILogger<ConsumeScopedServiceHostedService> _logger;

    public ConsumeScopedServiceHostedService(IServiceProvider services, 
        ILogger<ConsumeScopedServiceHostedService> logger)
    {
        Services = services;
        _logger = logger;
    }

    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service running.");

        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is working.");

        using (var scope = Services.CreateScope())
        {
            var scopedProcessingService = 
                scope.ServiceProvider
                    .GetRequiredService<IScopedProcessingService>();

            await scopedProcessingService.DoWork(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
```

The services are registered in IHostBuilder.ConfigureServices (Program.cs). The hosted service is registered with the AddHostedService extension method:

```csharp
services.AddHostedService<ConsumeScopedServiceHostedService>();
services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
```

#### Queued background tasks
A background task queue is based on the .NET 4.x QueueBackgroundWorkItem:

```csharp
public interface IBackgroundTaskQueue
{
    ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem);

    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken);
}

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    public BackgroundTaskQueue(int capacity)
    {
        // Capacity should be set based on the expected application load and
        // number of concurrent threads accessing the queue.            
        // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
        // which completes only when space became available. This leads to backpressure,
        // in case too many publishers/calls start accumulating.
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
    }

    public async ValueTask QueueBackgroundWorkItemAsync(
        Func<CancellationToken, ValueTask> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        await _queue.Writer.WriteAsync(workItem);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        var workItem = await _queue.Reader.ReadAsync(cancellationToken);

        return workItem;
    }
}
```

In the following QueueHostedService example:

* The BackgroundProcessing method returns a Task, which is awaited in ExecuteAsync.
* Background tasks in the queue are dequeued and executed in BackgroundProcessing.
* Work items are awaited before the service stops in StopAsync.

```csharp
public class QueuedHostedService : BackgroundService
{
    private readonly ILogger<QueuedHostedService> _logger;

    public QueuedHostedService(IBackgroundTaskQueue taskQueue, 
        ILogger<QueuedHostedService> logger)
    {
        TaskQueue = taskQueue;
        _logger = logger;
    }

    public IBackgroundTaskQueue TaskQueue { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"Queued Hosted Service is running.{Environment.NewLine}" +
            $"{Environment.NewLine}Tap W to add a work item to the " +
            $"background queue.{Environment.NewLine}");

        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = 
                await TaskQueue.DequeueAsync(stoppingToken);

            try
            {
                await workItem(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error occurred executing {WorkItem}.", nameof(workItem));
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
```
A MonitorLoop service handles enqueuing tasks for the hosted service whenever the w key is selected on an input device:

* The IBackgroundTaskQueue is injected into the MonitorLoop service.
* IBackgroundTaskQueue.QueueBackgroundWorkItem is called to enqueue a work item.
* The work item simulates a long-running background task:
    * Three 5-second delays are executed (Task.Delay).
    * A try-catch statement traps OperationCanceledException if the task is cancelled.

```csharp
public class MonitorLoop
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger _logger;
    private readonly CancellationToken _cancellationToken;

    public MonitorLoop(IBackgroundTaskQueue taskQueue,
        ILogger<MonitorLoop> logger,
        IHostApplicationLifetime applicationLifetime)
    {
        _taskQueue = taskQueue;
        _logger = logger;
        _cancellationToken = applicationLifetime.ApplicationStopping;
    }

    public void StartMonitorLoop()
    {
        _logger.LogInformation("MonitorAsync Loop is starting.");

        // Run a console user input loop in a background thread
        Task.Run(async () => await MonitorAsync());
    }

    private async ValueTask MonitorAsync()
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            var keyStroke = Console.ReadKey();

            if (keyStroke.Key == ConsoleKey.W)
            {
                // Enqueue a background work item
                await _taskQueue.QueueBackgroundWorkItemAsync(BuildWorkItem);
            }
        }
    }

    private async ValueTask BuildWorkItem(CancellationToken token)
    {
        // Simulate three 5-second tasks to complete
        // for each enqueued work item

        int delayLoop = 0;
        var guid = Guid.NewGuid().ToString();

        _logger.LogInformation("Queued Background Task {Guid} is starting.", guid);

        while (!token.IsCancellationRequested && delayLoop < 3)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5), token);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if the Delay is cancelled
            }

            delayLoop++;

            _logger.LogInformation("Queued Background Task {Guid} is running. " 
                                   + "{DelayLoop}/3", guid, delayLoop);
        }

        if (delayLoop == 3)
        {
            _logger.LogInformation("Queued Background Task {Guid} is complete.", guid);
        }
        else
        {
            _logger.LogInformation("Queued Background Task {Guid} was cancelled.", guid);
        }
    }
}
```

The services are registered in IHostBuilder.ConfigureServices (Program.cs). The hosted service is registered with the AddHostedService extension method:

```csharp
services.AddSingleton<MonitorLoop>();
services.AddHostedService<QueuedHostedService>();
services.AddSingleton<IBackgroundTaskQueue>(ctx =>
{
    if (!int.TryParse(hostContext.Configuration["QueueCapacity"], out var queueCapacity))
        queueCapacity = 100;
    return new BackgroundTaskQueue(queueCapacity);
});
```

MonitorLoop is started in Program.cs:

```csharp
var monitorLoop = host.Services.GetRequiredService<MonitorLoop>();
monitorLoop.StartMonitorLoop();
```

#### Asynchronous timed background task
The following code creates an asynchronous timed background task:

```csharp
namespace TimedBackgroundTasks;

public class TimedHostedService : BackgroundService
{
    private readonly ILogger<TimedHostedService> _logger;
    private int _executionCount;

    public TimedHostedService(ILogger<TimedHostedService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        // When the timer should have no due-time, then do the work once now.
        DoWork();

        using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                DoWork();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
        }
    }

    // Could also be a async method, that can be awaited in ExecuteAsync above
    private void DoWork()
    {
        int count = Interlocked.Increment(ref _executionCount);

        _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
    }
}
```

### [Worker services in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/workers?pivots=dotnet-6-0)

There are numerous reasons for creating long-running services such as:

* Processing CPU-intensive data.
* Queuing work items in the background.
* Performing a time-based operation on a schedule.

Background service processing usually doesn't involve a user interface (UI), but UIs can be built around them. With .NET, you can use the BackgroundService, which is an implementation of IHostedService, or implement your own.

With .NET, you're no longer restricted to Windows. You can develop cross-platform background services. Hosted services are logging, configuration, and dependency injection (DI) ready. They're a part of the extensions suite of libraries, meaning they're fundamental to all .NET workloads that work with the generic host.

#### Terminology

* **Background Service:** Refers to the BackgroundService type.
* **Hosted Service:** Implementations of IHostedService, or referring to the IHostedService itself.
* **Long-running Service:** Any service that runs continuously.
* **Windows Service:** The Windows Service infrastructure, originally .NET Framework centric but now accessible via .NET.
* **Worker Service:** Refers to the Worker Service template.

#### Worker Service template

The template consists of a Program and Worker class.

```csharp
using App.WorkerService;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    });

IHost host = builder.Build();
host.Run();
```
The preceding Program class:

* Creates a [HostApplicationBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostapplicationbuilder).
* Calls [AddHostedService](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionhostedserviceextensions.addhostedservice) to register the Worker as a hosted service.
* Builds an IHost from the builder.
* Calls Run on the host instance, which runs the app.


> By default the Worker template doesn't enable server garbage collection (GC). All of the scenarios that require long-running services should consider performance implications of this default. To enable server GC, add the ServerGarbageCollection node to the project file:
```xml
<PropertyGroup>
     <ServerGarbageCollection>true</ServerGarbageCollection>
</PropertyGroup>
```

As for the Worker, the template provides a simple implementation.

```csharp
namespace App.WorkerService;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1_000, stoppingToken);
        }
    }
}
```

The preceding Worker class is a subclass of BackgroundService, which implements IHostedService. The BackgroundService is an abstract class and requires the subclass to implement BackgroundService.ExecuteAsync(CancellationToken). In the template implementation, the ExecuteAsync loops once per second, logging the current date and time until the process is signaled to cancel.

#### The project file
The Worker template relies on the following project file Sdk:

```xml
<Project Sdk="Microsoft.NET.Sdk.Worker">
```

#### Containers and cloud adaptability
With most modern .NET workloads, containers are a viable option. When creating a long-running service from the Worker template in Visual Studio, you can opt in to Docker support. Doing so creates a Dockerfile that containerizes your .NET app. A Dockerfile is a set of instructions to build an image. For .NET apps, the Dockerfile usually sits in the root of the directory next to a solution file.

```Dockerfile
# See https://aka.ms/containerfastmode to understand how Visual Studio uses this
# Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["background-service/App.WorkerService.csproj", "background-service/"]
RUN dotnet restore "background-service/App.WorkerService.csproj"
COPY . .
WORKDIR "/src/background-service"
RUN dotnet build "App.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.WorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.WorkerService.dll"]
```

The preceding Dockerfile steps include:

* Setting the base image from mcr.microsoft.com/dotnet/runtime:6.0 as the alias base.
* Changing the working directory to /app.
* Setting the build alias from the mcr.microsoft.com/dotnet/sdk:6.0 image.
* Changing the working directory to /src.
* Copying the contents and publishing the .NET app:
* The app is published using the dotnet publish command.
* Relayering the .NET SDK image from mcr.microsoft.com/dotnet/runtime:6.0 (the base alias).
* Copying the published build output from the /publish.
* Defining the entry point, which delegates to dotnet App.BackgroundService.dll.

> The MCR in mcr.microsoft.com stands for "Microsoft Container Registry", and is Microsoft's syndicated container catalog from the official Docker hub. 

When you target Docker as a deployment strategy for your .NET Worker Service, there are a few considerations in the project file:

```xml
<Project Sdk="Microsoft.NET.Sdk.Worker">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>true</ImplicitUsings>
    <RootNamespace>App.WorkerService</RootNamespace>
    <DockerDefaultTargetOS>Linux</DockerDefaultTargetOS>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="7.0.1" />
    <PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.19.5" />
  </ItemGroup>
</Project>
```
In the preceding project file, the `<DockerDefaultTargetOS>` element specifies Linux as its target. To target Windows containers, use Windows instead. The [Microsoft.VisualStudio.Azure.Containers.Tools.Targets NuGet package](https://www.nuget.org/packages/Microsoft.VisualStudio.Azure.Containers.Tools.Targets) is automatically added as a package reference when Docker support is selected from the template.

For more information on Docker with .NET, see [Tutorial: Containerize a .NET app](https://learn.microsoft.com/en-us/dotnet/core/docker/build-container). For more information on deploying to Azure, see [Tutorial: Deploy a Worker Service to Azure](https://learn.microsoft.com/en-us/dotnet/core/extensions/cloud-service).

#### Signal completion
In most common scenarios, you don't need to explicitly signal the completion of a hosted service. When the host starts the services, they're designed to run until the host is stopped. In some scenarios, however, you may need to signal the completion of the entire host application when the service completes. To signal the completion, consider the following Worker class:

```csharp
namespace App.SignalCompletionService;

public sealed class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<Worker> _logger;

    public Worker(
        IHostApplicationLifetime hostApplicationLifetime, ILogger<Worker> logger) =>
        (_hostApplicationLifetime, _logger) = (hostApplicationLifetime, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // TODO: implement single execution logic here.
        _logger.LogInformation(
            "Worker running at: {time}", DateTimeOffset.Now);

        await Task.Delay(1000, stoppingToken);

        // When completed, the entire app host will stop.
        _hostApplicationLifetime.StopApplication();
    }
}
```

In the preceding code, the ExecuteAsync method doesn't loop, and when it's complete it calls IHostApplicationLifetime.StopApplication().

> This will signal to the host that it should stop, and without this call to StopApplication the host will continue to run indefinitely.


### [Running .NET Core Applications as a Windows Service](https://code-maze.com/aspnetcore-running-applications-as-windows-service/)

#### Windows Services in .NET Core
We may want to create long-running background services in .NET in specific scenarios. For instance, we might want to perform some processor-intensive tasks, queue some operations in the background or schedule some operations to execute at a later time. For all these purposes, we can make use of the BackgroundService class in .NET, which implements the IHostedService interface.

For implementing long-running services, we can create a class inheriting from the BackgroundService abstract class. Along with that, we’ll have to provide an implementation for the ExecuteAsync() method, which runs when the service starts. While implementing the ExecuteAsync() method, it should return a Task that represents the lifetime of the long-running operation. There is also an option to create our custom background service class by implementing the IHostedService interface if we wish to have more control over the background service functionality. 

The background services that we create in .NET are cross-platform and support the inbuilt .NET features like logging, configuration, dependency injection, etc.

#### Creating the Project
For creating background services, we can use the Worker Service Template that is available with both the .NET CLI and Visual Studio. Worker Services allows for running background services through the use of a hosted service.

```shell
dotnet new worker -o ContosoWorker
```

#### The Worker Service Template in .NET
A project we create using the worker service template will consist of 2 files – the Program class and the Worker class.

The Program class will contain the code to add the Worker class as a hosted service and run it:

```csharp
using ContosoWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
```

As we mentioned while explaining the windows services, any service that we implement should either inherit from the BackgroundService class or a custom implementation of it. Here, the Worker class contains the code for the service and it inherits from the BackgroundService class, which in turn implements the IHostedService interface:

```csharp
namespace ContosoWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}

```
An instance of ILogger is injected into the Worker class for the logging support. Additionally, there is the ExecuteAsync() method, which runs when the service starts. The default implementation of this method in the project template runs in a loop every second, logging the current date and time.

The Worker Service Template will provide the code for a simple background service and we can modify it to suit our requirements.

#### Configuring the Project
To have the support to host our application as a windows service, first, we need to install the Microsoft.Extensions.Hosting.WindowsServices NuGet package:

```shell
cd .\ContosoWorker\
dotnet add package Microsoft.Extensions.Hosting.WindowsServices
```

After that, we need to modify the Program class by adding the UseWindowsService() class:

```csharp
using ContosoWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Contoso Worker Service";
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

```

The UseWindowsService() extension method configures the application to work as a windows service. Along with that, we have set the service name using the options.ServiceName property.

Similarly, let’s modify the ExecuteAsync() method of the  Worker class to customize the log message:

```csharp
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Contoso Worker Service running at: {time}", DateTimeOffset.Now);
            await Task.Delay(30000, stoppingToken);
        }
    }
```

Along with that, we change the logging interval to 30 seconds as well. Now the service will log the message once every 30 seconds.

By default, the windows service will write logs into the Application Event Log and we can use the Event Viewer tool for viewing those. Also, by default, a windows service will write only logs of severity Warning and above into the Event Log. That said, we can configure this behavior in the appsettings file: 

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "EventLog": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  }
}

```
By adding a new section for the Event Log, we can change the default Log Level to Information, which will log the information as well.

#### Publishing the Project

To create the .NET Worker Service app as a Windows Service, it's recommended that you publish the app as a single file executable. It's less error-prone to have a self-contained executable, as there aren't any dependent files lying around the file system.

```xml
<Project Sdk="Microsoft.NET.Sdk.Worker">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <UserSecretsId>dotnet-ContosoWorker-606bcfc9-17f9-4079-9a8a-b0ff862621d4</UserSecretsId>
    <RootNamespace>ContosoWorker</RootNamespace>
    <OutputType>exe</OutputType>
    <PublishSingleFile Condition="'$(Configuration)' == 'Release'">true</PublishSingleFile>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <PlatformTarget>x64</PlatformTarget>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="6.0.1" />
    <PackageReference Include="Microsoft.Extensions.Hosting.WindowsServices" Version="7.0.1" />
    <PackageReference Include="TimeZoneConverter" Version="6.1.0" />
  </ItemGroup>
</Project>

```
The preceding highlighted lines of the project file define the following behaviors:

`<OutputType>exe</OutputType>`: Creates a console application.
`<PublishSingleFile Condition="'$(Configuration)' == 'Release'">true</PublishSingleFile>`: Enables single-file publishing.
`<RuntimeIdentifier>win-x64</RuntimeIdentifier>`: Specifies the RID of win-x64.
`<PlatformTarget>x64</PlatformTarget>`: Specify the target platform CPU of 64-bit.

You could use the .NET CLI to publish the app:

```shell
dotnet publish --output "C:\custom\publish\directory"
```
This will produce a standalone executable output of the service in the specified folder location.

#### Creating the Windows Service

For creating a Windows Service, we can use the Windows Service Control Manager (sc.exe) tool. The service control manager operations require higher permissions as we are working directly with the operating system and hence we need to run the commands in a Windows PowerShell console with Administrator privilege.

In the PowerShell console, we can use the sc.exe create command and provide the service name and path as arguments:

```powershell
sc.exe create "Contoso Service" binpath="C:\service\ContosoWorkerService.exe"
```

Once the command executes successfully, it will create a new windows service with the name Code-Maze Service and return the output:

        [SC] CreateService SUCCESS

We can verify the newly created service in the Windows Service Management Console:

<img src="https://code-maze.com/wp-content/uploads/2022/06/windows-service-running.png"/>

By default, the service might be in the stopped state and we will have to start it manually by using the sc.exe start command:

```powershell
sc.exe start "Code-Maze Service"
```

Once the command executes successfully, it will provide an output similar to this one:

```shell
SERVICE_NAME: Code-Maze Service
        TYPE               : 10  WIN32_OWN_PROCESS
        STATE              : 2  START_PENDING
                                (NOT_STOPPABLE, NOT_PAUSABLE, IGNORES_SHUTDOWN)
        WIN32_EXIT_CODE    : 0  (0x0)
        SERVICE_EXIT_CODE  : 0  (0x0)
        CHECKPOINT         : 0x0
        WAIT_HINT          : 0x7d0
        PID                : 6720
        FLAGS
```

This will start the windows service and it will continue to run in the background.

#### Verifying the Windows Service
Now we are going to verify that the windows service works as expected. For that, let’s open the Event Viewer.

Remember that we implemented the service to write a log once every minute. Within the Event Viewer, we can find the logs in the Windows Logs -> Application node. We are going to see a bunch of events related to our service there:

<img src="https://code-maze.com/wp-content/uploads/2022/06/windows-services-event-log-e1655967615853.png"/>

As soon as the service starts, the Windows Service Manager logs an event with the source as the service name. The first event with the source name Code-Maze Service corresponds to that. We can verify this by opening that event. The event details will contain the corresponding message and details:

<img src="https://code-maze.com/wp-content/uploads/2022/06/windows-service-started-event.png"/>

Apart from that, while the service is running, it logs an event every minute with the source matching the app’s namespace. All the subsequent events with the source name CodeMazeWorkerService correspond to those. We can verify this by opening those events. Those events will contain the message that the service logs:

<img src="https://code-maze.com/wp-content/uploads/2022/06/windows-service-log-event.png"/>

#### Removing the Windows Service
Once we create a windows service, it keeps on running in the background. For removing a windows service from the system, we have to first stop it and then delete it.

For stopping a windows service, we can use the sc.exe stop command: 

```powershell
sc.exe stop "Code-Maze Service"
```

This will stop the service and provide a similar response:

```shell
SERVICE_NAME: Code-Maze Service
        TYPE               : 10  WIN32_OWN_PROCESS
        STATE              : 3  STOP_PENDING
                                (STOPPABLE, NOT_PAUSABLE, ACCEPTS_SHUTDOWN)
        WIN32_EXIT_CODE    : 0  (0x0)
        SERVICE_EXIT_CODE  : 0  (0x0)
        CHECKPOINT         : 0x0
        WAIT_HINT          : 0x0
```

Even though this will stop the service, we can still find the service in the Services Console. This is particularly useful when we just need to stop the service temporarily and may want to start it later.

On the other hand, if we no longer need the service, we can delete it using the sc.exe delete command:

````powershell
sc.exe delete "Code-Maze Service"
````

This will remove the service from the system and give the response:

````shell
[SC] DeleteService SUCCESS
````

Now if we check the Services Console, we cannot find the service as it will be completely removed from the system.

### Implementation

Good resource: https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service?pivots=dotnet-6-0

#### Create the project and add the packages

```shell
dotnet new worker -o ContosoWorker
cd .\ContosoWorker\
dotnet add package Microsoft.Extensions.Hosting.WindowsServices
dotnet add package TimeZoneConverter
```

Write the code

#### Publish the app

Add the following to the csproj file:

```xml
<Project Sdk="Microsoft.NET.Sdk.Worker">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <UserSecretsId>dotnet-ContosoWorker-606bcfc9-17f9-4079-9a8a-b0ff862621d4</UserSecretsId>
    
    <RootNamespace>ContosoWorker</RootNamespace>
    <OutputType>exe</OutputType>
    <PublishSingleFile Condition="'$(Configuration)' == 'Release'">true</PublishSingleFile>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <PlatformTarget>x64</PlatformTarget>

  </PropertyGroup>
```
The preceding highlighted lines of the project file define the following behaviors:

`<OutputType>exe</OutputType>`: Creates a console application.
`<PublishSingleFile Condition="'$(Configuration)' == 'Release'">true</PublishSingleFile>`: Enables single-file publishing.
`<RuntimeIdentifier>win-x64</RuntimeIdentifier>`: Specifies the RID of win-x64.
`<PlatformTarget>x64</PlatformTarget>`: Specify the target platform CPU of 64-bit.

You could use the .NET CLI to publish the app:

```shell
dotnet publish -c Release -r win-x64 --self-contained true
```

#### Create the Windows Service

To create the Windows Service, use the native Windows Service Control Manager's (sc.exe) create command. Run PowerShell as an Administrator.

```powershell
sc.exe create "Contoso Background Service" binpath="C:\Users\laura.bustamanteh\Downloads\dotnet-ramp-up\ContosoWorker\bin\Release\net6.0\win-x64\publish\ContosoWorker.exe"
```
<img src="media\create-windows-service.png" width=700px/>

We can verify the newly created service in the Services app:

<img src="media\contoso-background-service-services-manager.png" width=700px/>

To verify that the service is functioning as expected, you need to:

* Start the service
* View the logs
* Stop the service

#### Start the Windows Service
To start the Windows Service, use the sc.exe start command:

```powershell
sc.exe start "Contoso Background Service"
```

Output:

```shell
SERVICE_NAME: Contoso Background Service
        TYPE               : 10  WIN32_OWN_PROCESS
        STATE              : 2  START_PENDING
                                (NOT_STOPPABLE, NOT_PAUSABLE, IGNORES_SHUTDOWN)
        WIN32_EXIT_CODE    : 0  (0x0)
        SERVICE_EXIT_CODE  : 0  (0x0)
        CHECKPOINT         : 0x0
        WAIT_HINT          : 0x7d0
        PID                : 21056
        FLAGS              :
```
The service Status will transition out of START_PENDING to Running.

#### View logs
To view logs, open the Event Viewer. Search for "Event Viewer" app. Select the Event Viewer (Local) > Windows Logs > Application node. You should see a Warning level entry with a Source matching the apps namespace. Double-click the entry, or right-click and select Event Properties to view the details.

And we can see we get the expected output.

<img src="media\contoso-background-service-event-viewer-result.png" width=700px/>


#### Stop the Windows Service
To stop the Windows Service, use the sc.exe stop command:

```powershell
sc.exe stop "Contoso Background Service"
```


## 9. Authentication JWT

In this folder, you will find a solution with a configured application with the code base of a web API service that manages the user lifecycle operations.

<img src="media\authetication-service.png" width=700px>

The web app emulates a user service that contains seed users. A user is defined in the following way

``` csharp
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserRole Role { get; set; }
    public bool IsActiveRole { get; set; } = true;
}
```

1. **Requirement**

​​
The requirement is to make the endpoints protected by using an authentication schema by using JWT Tokens. In order to make the web API secure you will need to create a new endpoint to log in, the response of this endpoint is a JWT token that will contain the user claims information.

The token should contain in the claims the following information.

* Name
* Email
* User role
* IsActiveRole flag

Once the token is generated it would be required to protect the user endpoints, which means that in order to reach those endpoints you will need to provide it as part of the request the token.

2. **Requirement**
​​​​

Once you have the endpoints protected it will be needed to make them protected based on the user role. As part of a requirement here is that the endpoints might be accessed based on the role of the user that is trying to hit it. Once you have the endpoints protected it will be needed to make them protected based on the user role. As part of a requirement here is that the endpoints might be accessed based on the role of the user that is trying to hit it.

The defined roles are described in the following enumeration.

``` csharp
public enum UserRole
{
    Reader,
    Contributor,
    Manager
}
```
​​​​​​​​​The rules are defined in the following way.

* Reader: Can list users and get users by id.
* Contributor: Same reader permissions also can create and update users.
* Manager: Same contributor permissions plus can delete users.



### Related material

<ul>
    <li><a data-cke-saved-href="https://code-maze.com/authentication-aspnetcore-jwt-1/" href="https://code-maze.com/authentication-aspnetcore-jwt-1/">JWT Authentication in ASP.NET Core Web API</a></li>
    <li><a data-cke-saved-href="https://weblog.west-wind.com/posts/2021/Mar/09/Role-based-JWT-Tokens-in-ASPNET-Core" href="https://weblog.west-wind.com/posts/2021/Mar/09/Role-based-JWT-Tokens-in-ASPNET-Core">Role based JWT Tokens in ASP.NET Core APIs</a></li>
    <li><a data-cke-saved-href="https://www.c-sharpcorner.com/article/jwt-json-web-token-authentication-in-asp-net-core/" href="https://www.c-sharpcorner.com/article/jwt-json-web-token-authentication-in-asp-net-core/">JWT in ASP.NET Core</a></li>
</ul>


### [JWT Authentication in ASP.NET Core Web API](https://code-maze.com/authentication-aspnetcore-jwt-1/)

There is an application that has a login form. A user enters their username, and password and presses the login button. After pressing the login button, a client (eg web browser) sends the user’s data to the server’s API endpoint:

<img src="https://code-maze.com/wp-content/uploads/2018/04/picture_1-e1650647612114.png" width=700px>

When the server validates the user’s credentials and confirms that the user is valid, it’s going to send an encoded JWT to the client. JSON web token is basically a JavaScript object that can contain some attributes of the logged-in user. It can contain a username, user subject, user roles, or some other useful information.

On the client-side, we store the JWT in the browser’s storage to remember the user’s login session. We may also use the information from the JWT to enhance the security of our application as well.

### What is JWT (JSON Web Token)
JSON web tokens enable a secure way to transmit data between two parties in the form of a JSON object. It’s an open standard and it’s a popular mechanism for web authentication. In our case, we are going to use JSON web tokens to securely transfer a user’s data between the client and the server.

JSON web tokens consist of three basic parts: the header, payload, and signature.

One real example of a JSON web token:

<img src="https://code-maze.com/wp-content/uploads/2018/04/2.png" width=500px>

* Header: JSON object encoded in the base64 format. It contains information like the type of token and the name of the algorithm
    ``` json
    { 
        "alg": "HS256", 
        "typ": "JWT" 
    }
    ```

* Payload: is also a JavaScript object encoded in the base64 format. The payload contains some attributes about the logged-in user. For example, it can contain the user id, user subject, and information about whether a user is an admin user or not. JSON web tokens are not encrypted and can be decoded with any base64 decoder so we should never include sensitive information in the Payload

    ``` json
    { 
        "sub": "1234567890", 
        "name": "John Doe", 
        "iat": 1516239022 
    }
    ```

* Signature: Usually, the server uses the signature part to verify whether the token contains valid information – the information the server is issuing. It is a digital signature that gets generated by combining the header and the payload together. Moreover, it’s based on a secret key that only the server knows


    <img src="https://code-maze.com/wp-content/uploads/2018/04/3.png" width=300px>

    So, if malicious users try to modify the values in the payload, they have to recreate the signature and for that purpose, they need the secret key that only the server knows about. On the server side, we can easily verify if the values are original or not by comparing the original signature with a new signature computed from the values coming from the client.

### Creating ASP.NET Core Web API Project > Configuring JWT Authentication

1. Create a brand new ASP.NET Core Web API project.

2. We can open the launchSettings.json file and modify the applicationUrl property:

        "applicationUrl": "https://localhost:5001;http://localhost:5000"

    As a result, we will see our application hosted at https://localhost:5001

3. To configure JWT authentication in .NET Core, we need to modify Program.csfile.

    > For the sake of simplicity, we are going to add all the code inside the Program class. But the better practice is to use [Extension methods](https://code-maze.com/csharp-static-members-constants-extension-methods/) so we could free our class from extra code lines. If you want to learn how to do that, and to learn more about configuring the .NET Core Web API project, check out: [.NET Core Service Configuration](https://code-maze.com/net-core-web-development-part2/).

    1. First, let’s install the Microsoft.AspNetCore.Authentication.JwtBearer NuGet package that we require to work with JWT in the ASP.NET Core app:

        `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer`

    2. Next, let’s add the code to configure JWT right above the builder.Services.AddControllers() line:

        ```csharp
        builder.Services.AddAuthentication(opt => {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:5001",
                    ValidAudience = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
                };
            });
        ```

        In order for this to work, we have to add a few using directives:

        ```csharp
        using Microsoft.AspNetCore.Authentication.JwtBearer;
        using Microsoft.IdentityModel.Tokens;
        using System.Text;
        ```

        **Explanation:**

        Firstly, we register the JWT authentication middleware by calling the AddAuthentication method. Next, we specify the default authentication scheme JwtBearerDefaults.AuthenticationScheme as well as DefaultChallengeScheme.

        By calling the AddJwtBearer method, we enable the JWT authenticating using the default scheme, and we pass a parameter, which we use to set up JWT bearer options:

        * The issuer is the actual server that created the token (ValidateIssuer=true)
        * The receiver of the token is a valid recipient (ValidateAudience=true)
        * The token has not expired (ValidateLifetime=true)
        * The signing key is valid and is trusted by the server (ValidateIssuerSigningKey=true)
        * Additionally, we are providing values for the issuer, audience, and the secret key that the server uses to generate the signature for JWT.

        We are going to hardcode both username and password for the sake of simplicity. But, the best practice is to put the credentials in a database or a configuration file or to store the secret key in the environment variable.

    3. There is one more step we need to do to make our authentication middleware available to the application:

        ```csharp
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        ```

### Securing API Endpoints

We already have an API endpoint /weatherforecast to get some example weather information and that endpoint is not secure. Anyone can send a request to https://localhost:5001/weatherforecast to fetch the values. So, in this section, we are going to add a new api/customers endpoint to serve a list of the customers. This endpoint is going to be secure from anonymous users and only logged-in users will be able to consume it.

1. Now, let’s add an empty CustomersController in the Controllers folders. Inside the controller, we are going to add a Get action method that is going to return an array of customers. More importantly, we are going to add an extra security layer by decorating the action method with the `[Authorize]` attribute so only logged-in users can access the route:

    ```csharp
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet, Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "John Doe", "Jane Doe" };
        }
    }
    ```

    To be able to use the Authorize attribute we have to add a new using directive inside the file:

    ```csharp
    using Microsoft.AspNetCore.Authorization;
    ```

    `Authorize` attribute on top of the GET method restricts access to only authorized users. Only users who are logged in can fetch the list of customers. Therefore, this time if we make a request to https://localhost:5001/api/customers from the browser’s address bar, instead of getting a list of customers, we are going to get a 401 Not Authorized response:


### Adding Login Endpoint

To authenticate anonymous users, we have to provide a login endpoint so the users can log in and access protected resources. A user is going to provide a username and password, and if the credentials are valid we are going to issue a JSON web token for the requesting client.

In addition, before we start implementing the authentication controller, we need to add a LoginModel to hold user’s credentials on the server. LoginModel is a simple class that contains two properties: UserName and Password.  We are going to create a Models folder in the root directory and inside it a LoginModel class:

```csharp
public class LoginModel
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}
```

Also, let’s create one more class inside the same Models folder:

```csharp
public class AuthenticatedResponse
{
    public string? Token { get; set; }
}
```

Now let’s create the AuthController inside the Controllers folder.

Inside the AuthControllerwe are going to add the Login action to validate the user’s credentials. If the credentials are valid, we are going to issue a JSON web token. For this demo, we are going to hardcode the username and password to implement a fake user. After validating the user’s credentials we are going to generate a JWT with a secret key. JWT uses the secret key to generate the signature.

So, let’s implement the AuthController:

```csharp
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel user)
    {
        if (user is null)
        {
            return BadRequest("Invalid client request");
        }

        if (user.UserName == "johndoe" && user.Password == "def@123")
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return Ok(new AuthenticatedResponse { Token = tokenString });
        }

        return Unauthorized(); // 401
    }
}
```

1. We decorate our Login action with the HttpPost attribute. Inside the login method, we create the SymmetricSecretKey with the secret key value superSecretKey@345. Then, we create the SigningCredentials object and as arguments, we provide a secret key and the name of the hashing algorithm that we are going to use to encode the token.

2. We create the JwtSecurityToken object with some important parameters:

    * Issuer: The first parameter is a simple string representing the name of the webserver that issues the token
    * Audience: The second parameter is a string value representing valid recipients
    * Claims: The third argument is a list of user roles, for example, the user can be an admin, manager, or author (we are going to add roles in the next post)
    * Expires: The fourth argument is the DateTime object that represents the date and time after which the token expires

3. Then, we create a string representation of JWT by calling the WriteToken method on JwtSecurityTokenHandler. Finally, we return JWT in a response. As a response, we create the AuthenticatedResponse object that contains only the Token property.

### Testing the JWT Authentication

1. let’s send a POST request to https://localhost:5001/api/auth/login and provide a request body:

    ```json
    { 
        "UserName":"johndoe", 
        "Password": "def@123" 
    }
    ```

    In the response section, we are going to see a 200 OK response with the JWT string in the response body:

    <img src="https://code-maze.com/wp-content/uploads/2019/11/02-Postam-Jwt-Response.png" width=700px/>

### Role-Based Authorization (https://code-maze.com/authentication-aspnetcore-jwt-2/)
Because we have only the `[Authorize]` attribute on top of the Customers controller’s GET action, all the authenticated users have access to that endpoint.

First, let’s modify the `[Authorize]` attribute to give access only to a user with the Manager role:

```csharp
[HttpGet, Authorize(Roles = "Manager")]
public IEnumerable<string> Get()
{
    return new string[] { "John Doe", "Jane Doe" };
}
```

Additionally, let’s modify the Login action in the AuthController to set up the user claims:

```csharp
if (user.UserName == "johndoe" && user.Password == "def@123")
{
    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim> //
    { //
        new Claim(ClaimTypes.Name, user.UserName), //
        new Claim(ClaimTypes.Role, "Manager") //
    };//

    var tokeOptions = new JwtSecurityToken(
        issuer: "https://localhost:5001",
        audience: "https://localhost:5001",
        claims: claims, //
        expires: DateTime.Now.AddMinutes(5),
        signingCredentials: signinCredentials
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

    return Ok(new AuthenticatedResponse { Token = tokenString });
}
```

In the changed parts of the code, we create claims by adding the username and the role claim to the Claim list. Now, our “johndoe” user is a Manager and it should have access to the Customer’s GET action. These claims are going to be included in our token. 

> All the JWT-related logic is inside our Login method for the sake of simplicity. But we encourage you to create a new class (JwtConfigurator or use any other name) and transfer all the SymmetricSecurityKey, SigninCredentials, Claims, and JWtSecurityToken logic to a new class.

### JwtHelper DecodeToken

The jwtHelper service has the decodeToken function which can decode our token into the JSON object. Because we have already injected the JwtHelper service into the AuthGuard service, let’s modify that service a bit just to see how the decodeToken function works. We are going to add one line of code that checks if the token exists and if it hasn’t expired:

```csharp
if (token && !this.jwtHelper.isTokenExpired(token)){
  console.log(this.jwtHelper.decodeToken(token))
  return true;
}
```

Once we log in again, we can see the result in the console window:

```json
aud: "https://localhost:5001"
exp: 1650718465
http://schemas.microsoft.com/ws/2008/06/identity/claims/role: "Manager"
http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name: "johndoe"
iss: "https://localhost:5001"
```

### [JWT Authentication In ASP.NET Core](https://www.c-sharpcorner.com/article/jwt-json-web-token-authentication-in-asp-net-core/)

### Implementation

1. In order to create the project:

    ``` shell
    mkdir PRFTLatam.Training.JwtAuthentication
    cd .\PRFTLatam.Training.JwtAuthentication\
    dotnet new webapi -o PRFTLatam.Training.JwtAuthentication.  Service
    dotnet new sln -o PRFTLatam.Training.JwtAuthentication
    dotnet sln add ./PRFTLatam.Training.JwtAuthentication.  Service/
    ```

2. Then create an Enums folder to add the different roles and a UserRole.cs file to add the roles

3. Create a Models folder to add the user model, the Dtos folder for the userDto, the services folder for the interfaces and implementations and add to the controllers folder the users controller

4. Add the dependency injection to the Program.cs for the services

5. Install package (version 6 is compatible with .net version 6):

    `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer -v 6.0`

6. Register JWT Middleware at Program.cs

    ```csharp
    // register the JWT authentication middleware
    builder.Services.AddAuthentication(opt => {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options => // enable the JWT authenticating
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // server that created the token
            ValidateAudience = true, // receiver of the token is a valid recipient
            ValidateLifetime = true, // token has not expired
            ValidateIssuerSigningKey = true, // signing key is valid and is trusted by the server
            ValidIssuer = "https://localhost:5001",
            ValidAudience = "https://localhost:5001",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
        };
    });
    ```

7. Create Login and AuthenticatedResponse models:

    **Login model**
    ```csharp
    namespace PRFTLatam.Training.JwtAuthentication.Service.Models;

    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    ```

    **Authenticated Response model**
    ```csharp
    using System.Security.Claims;

    namespace PRFTLatam.Training.JwtAuthentication.Service.Models;

    public class AuthenticatedResponse
    {
        public string Token { get; set; }
        public Dictionary<string, string> UserClaims { get; set; }
    }
    ```

8. Create the authentication controller for the login

9. Update endpoints with the `Authorize(Roles = "Reader,Contributor,Manager")` attribute to protect them, so that only logged in users can use them and also only the users with specific roles

10. In order to make requests, run the `dotnet run` command and use postman. Examples:

    * First, you must login with the post endpoint: https://localhost:5001/api/Auth/Login
        And add the user in the body:

        ```json
        // {
        //   "email": "john@test.com",
        //   "password": "123456"
        // }

        {
            "email": "karl@test.com",
            "password": "qwerty"
        }
        ```
        <img src="media\login-postman.png" width=700px/>

    * Then as karl can get users, we must pass the token received for Authorization: https://localhost:5001/api/Users/GetUsers

        <img src="media\getusers-postman.png" width=700px/>

    * To get a user by id, you must pass the id as part of the url: https://localhost:5001/api/Users/GetUserById/d1f760c6-d588-4648-8fb2-0dc727cac874

        <img src="media\getuserbyid-postman.png" width=700px/>

    * To create a user, in the body there must go the information for the new user: https://localhost:5001/api/Users/CreateUser
        ```json
        {
          "name": "Paolo",
          "email": "paolo@test.com",
          "password": "P40L0",
          "role": 1
        }
        ```
        <img src="media\createuser-postman.png" width=700px/>

    * To update an user, it's the same way as before: https://localhost:5001/api/Users/UpdateUser/5fce9472-cb47-48cc-a949-e17a65b1942b

        **Before:**
        ```json
        {
            "id": "84709e52-cedd-40fe-bbf9-fe7312cc95d3",
            "name": "Sammy Silva",
            "email": "sammy@test.com",
            "password": "s4mmy",
            "createdAt": "2023-09-01T14:24:44.99644-05:00",
            "role": 2,
            "isActiveRole": true
        }
        ```

        **After:**
        ```json
        {
            "name": "Sammy Silva",
            "email": "sammy@test.com",
            "password": "s4mmy",
            "role": 0
        }
        ```
        <img src="media\updateuser-postman.png" width=700px/>

    * Then to delete a user, the user must be a manager, with the current user, it is not possible, so it gives a 403 forbidden response: https://localhost:5001/api/Users/DeleteUser/5fce9472-cb47-48cc-a949-e17a65b1942b

        <img src="media\deleteuser-403forbidden-postman.png" width=700px/>

        But when logging in as the manager: https://localhost:5001/api/Users/DeleteUser/98257d99-2348-4502-886d-722056e479ff
        ```json
        {
            "email": "sammy@test.com",
            "password": "s4mmy"
        }
        ```
        This is the response:

        <img src="media/deleteuser-200ok-postman.png" width=700px/>

        And if we see all the users, we can see that the user with this id, changed the bool variable IsActiveRole to false:

        <img src="media/getusers-afterdelete-postman.png" width=700px/>






# Props

```csharp

```

```powershell

```
<img src="" width=700px/>

# QUESTIONS

1. No entiendo cómo el enunciado del ejercicio que piden hacer en la semana 3 de la web api

* Endpoint que dice que el sistema está arriba, devuelve ok
* Endpoint /identity, lee los ids desde un csv, columna con los id, devuelve cuales están malos con esos errores

* infra: clase que lee archivos y devuelve lista de archivos

* servicios: logica de validación de los ids, inyectar otro componente que devuelve la información de un archivo, llama a una interfaz que le devuelve los ids

2. 

    SQL Server express



escribir en comentarios el flujo
leer sobre algo que no sepa o me interese. qué pasa cuando hago un try catch

Aprender que algo se aprenda bien, bases conceptuales claras


4. La conexión a BD, en el program.cs y con el sql server como tal (buscar primero igual)

5. Añadí referencias de API con Infrastructure y Services, para poder hacer la inyección de dependecias con el UnitOfWork. Además los modelos los hice en Infrastructura en lugar de Services

6. Para esto `GET api/v1/getAllCustomersWithNoOrder Returns all customers who have never had an order.`, como en client no hay una asociación directa con orders sino que la clave foránea la tiene order para con los clientes, ¿cómo puedo obtener esos clientes? ¿haciendo un join de las dos tablas y mirar qué clientes no están en la tabla de orders? 


# Curso: Pruebas unitarias con xUnit en .NET

## Creación del proyecto

```shell
mkdir UnitTests.TextManager
cd .\UnitTests.TextManager\
mkdit TextManager
cd .\TextManager\
dotnet new classlib

cd ..
mkdir TextManager.Tests
cd .\TextManager.Tests\
dotnet new xunit
```

Add references:

```shell
dotnet add TextManager.Tests/ reference TextManager/
```

### [XUnit docs](https://xunit.net/docs/comparisons)

### Test Setup

Para configurar un método inicial que se va a ejecutar antes de cualquier prueba dentro de una clase de pruebas, se debe usar el constructor de la clase

Si se usa una misma variable en varios tests, la inicialización de la misma se puede hacer en el constructor, y así no habría tanta repetición

* El atributo `[Theory]` permite modificar parámetros (es mejor que sea para una poca cantidad de parámetros)

    <img src="media/xunit-inline-data-tests.png" width=700px />

* Pero se puede usar algo llamado `ClassData` que permite inicializar una mayor cantidad de parámetros más facilmente

    ```csharp
    using System.Collections;

    namespace TextManager.Tests;

    public class TextManagerClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {"", 0};
            yield return new object[] {"Hola mundo", 2};
            yield return new object[] {"Saludos a todos desde el curso de xunit", 8};
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); // reasignar el método con el que se acabó de crear
    }
    ```

    * Se puede llegar a necesitar que se salten algunas pruebas porque ya no se necesitan o porque se deben terminar luego, para eso se usa el parámetro `Skip` en el atributo Fact:

    Ejemplo:

    ```csharp
    [Fact(Skip="This test is not valid for the current code")]
    public void FindExactWord()
    {
        // Arrange
        // var textManager = new TextManager("Hola hola desde xunit");

        // Act
        var result = textManagerGlobal.FindExactWord("mundo", bolIgnoreUppercaseLowercase: true);

        // Assert
        Assert.IsType<List<Match>>(result); //must be of type List<Match>
    }
    ```

    Y se puede ver que no se ejecutó así:

    <img src="media/skip-test.png" />

### Mocks

El objetivo es reemplazar dependencias de un componente como un servicio o una clase, entonces se usan mocks que hacen creer al componente que usa el servicio que esta usando el real, entonces el mock devuelve un valor por defecto que se configura

Como las pruebas deber ser independientes, no deberían tener una dependencia de configuración específica, por tanto no tiene sentido hacer una prueba sobre una bd por ejemplo, ya que el objetivo es probar la lógica del código y no lo que hacen los servicios externos

https://nsubstitute.github.io/

https://nsubstitute.github.io/help/getting-started/

https://www.nuget.org/packages/NSubstitute#versions-body-tab

Ponerla en el csproj:

```xml
<PackageReference Include="NSubstitute" Version="5.1.0" />
```

Y el otro paquete:

```shell
dotnet add package NSubstitute.Analyzers.CSharp
```

* Para mirar las diferencias entre Moq y NSubstitute.
https://dev.to/cloudx/moq-vs-nsubstitute-who-is-the-winner-40gi

Con el objetivo de simular mejor el mockin por medio de inyección de dependencias: 

* Instalar la librería en el Proyecto de TextManager: `<PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="7.0.1" />`


### Opciones para medir la cobertura de las pruebas

Esto hace referencia a qué tanto código del proyecto que se está probando ha sido cubierto

#### [Coverlet](https://github.com/coverlet-coverage/coverlet)

Librería opensource que se puede usar para obtener la cobertura de código del proyecto
Permite generar reportes y se puede configurar la forma en la que se hace la cobertura (como excluyendo alguna clase)

Instalar la librería en Tests: 

```shell
dotnet add package coverlet.collector
dotnet add package coverlet.msbuild
```

Correr el siguiente comando para obtener la cobertura:

```shell
dotnet test /p:CollectCoverage=true
```

After the above command is run, a coverage.json file containing the results will be generated in the root directory of the test project. A summary of the results will also be displayed in the terminal.

Y el resultado obtenido es:

```shell
Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
[xUnit.net 00:00:00.59]     TextManager.Tests.TextManagerTest.FindExactWord [SKIP]
  Skipped TextManager.Tests.TextManagerTest.FindExactWord [1 ms]

Passed!  - Failed:     0, Passed:    12, Skipped:     1, Total:    13, Duration: 41 ms - TextManager.Tests.dll (net6.0)

Calculating coverage result...
  Generating report 'C:\Users\laura.bustamanteh\Downloads\dotnet-ramp-up\UnitTests.TextManager\TextManager.Tests\coverage.json'

+-------------+--------+--------+--------+
| Module      | Line   | Branch | Method |
+-------------+--------+--------+--------+
| TextManager | 42.47% | 31.25% | 63.63% |
+-------------+--------+--------+--------+

+---------+--------+--------+--------+
|         | Line   | Branch | Method |
+---------+--------+--------+--------+
| Total   | 42.47% | 31.25% | 63.63% |
+---------+--------+--------+--------+
| Average | 42.47% | 31.25% | 63.63% |
+---------+--------+--------+--------+
```

#### Configuración del reporte de cobertura

* Con el comando:

    ```shell
    dotnet test --no-build /p:CollectCoverage=true
    ```

    Se evita que se compile nuevamente la aplicación y que se ejecute más rápido el comando que obtiene la cobertura

* Parámetro **Include**:

    Permite especificar que namespaces se quieren incluir dentro de las pruebas, para esto se debe crear un archivo en el TextManager que será una clase es un namespace diferente, que no hace nada:

    ```csharp
    namespace TextTest;

    class ClassTest
    {
        public void Method1()
        {

        }

        public void Method2()
        {

        }
    }
    ```
    Y si se ejecuta el comando: `dotnet test /p:CollectCoverage=true`

    Se evidencia un resultado donde los porcentajes son menores, lo que quiere decir que no se están cubriendo esos métodos que se acabaron de agregar.

    ```shell
    Starting test execution, please wait...
    A total of 1 test files matched the specified pattern.
    [xUnit.net 00:00:00.57]     TextManager.Tests.TextManagerTest.FindExactWord [SKIP]
      Skipped TextManager.Tests.TextManagerTest.FindExactWord [1 ms]

    Passed!  - Failed:     0, Passed:    12, Skipped:     1, Total:    13, Duration: 48 ms - TextManager.Tests.dll (net6.0)

    Calculating coverage result...
      Generating report 'C:\Users\laura.bustamanteh\Downloads\dotnet-ramp-up\UnitTests.TextManager\TextManager.Tests\coverage.json'

    +-------------+--------+--------+--------+
    | Module      | Line   | Branch | Method |
    +-------------+--------+--------+--------+
    | TextManager | 41.02% | 31.25% | 53.84% |
    +-------------+--------+--------+--------+

    +---------+--------+--------+--------+
    |         | Line   | Branch | Method |
    +---------+--------+--------+--------+
    | Total   | 41.02% | 31.25% | 53.84% |
    +---------+--------+--------+--------+
    | Average | 41.02% | 31.25% | 53.84% |
    +---------+--------+--------+--------+
    
    ```

    Por tanto si se quiere indicar que solo se quiere usar el namespace TextManager y no el namespace TextTest, se hace el comando:

    ```shell
    dotnet test /p:CollectCoverage=true /p:Include="[*]TextManager.*"
    ```

    Para incluir cualquier namespace que se llame así, y así se obtiene el mismo reporte inicial:

    ```shell
    Calculating coverage result...
      Generating report 'C:\Users\laura.bustamanteh\Downloads\dotnet-ramp-up\UnitTests.TextManager\TextManager.Tests\coverage.json'

    +-------------+--------+--------+--------+
    | Module      | Line   | Branch | Method |
    +-------------+--------+--------+--------+
    | TextManager | 42.47% | 31.25% | 63.63% |
    +-------------+--------+--------+--------+

    +---------+--------+--------+--------+
    |         | Line   | Branch | Method |
    +---------+--------+--------+--------+
    | Total   | 42.47% | 31.25% | 63.63% |
    +---------+--------+--------+--------+
    | Average | 42.47% | 31.25% | 63.63% |
    +---------+--------+--------+--------+
    ```

* Parámetro **`[ExcludeFromCodeCoverage]`** para exluir métodos o clases específicas del análisis de cobertura, como en este caso:

    ```csharp
    using System.Diagnostics.CodeAnalysis;

    namespace TextTest;

    [ExcludeFromCodeCoverage]
    class ClassTest
    {
        public void Method1()
        {

        }

        public void Method2()
        {

        }
    }
    ```

    Y ya cuando se corre el mismo comando: `dotnet test /p:CollectCoverage=true`

    Obtenemos un mayor porcentaje de cobertura:

    ```shell
    Calculating coverage result...
      Generating report 'C:\Users\laura.bustamanteh\Downloads\dotnet-ramp-up\UnitTests.TextManager\TextManager.Tests\coverage.json'

    +-------------+--------+--------+--------+
    | Module      | Line   | Branch | Method |
    +-------------+--------+--------+--------+
    | TextManager | 56.47% | 41.66% | 77.77% |
    +-------------+--------+--------+--------+

    +---------+--------+--------+--------+
    |         | Line   | Branch | Method |
    +---------+--------+--------+--------+
    | Total   | 56.47% | 41.66% | 77.77% |
    +---------+--------+--------+--------+
    | Average | 56.47% | 41.66% | 77.77% |
    +---------+--------+--------+--------+
    
    ```

    Si se quiere especificar el atributo que permite excluir, o si se crea un atributo propio, se usa el comando más explicitamente así: 

    ```shell
    dotnet test /p:CollectCoverage=true /p:ExcludeByAttribute="ExcludeFromCodeCoverage"
    ```

    Obteniendo el mismo resultado anterior

* Sobre el reporte de cobertura:
    Comando que permite generarlo en el formato cobertura que devuelve algo visualmente diferente en xml:

    ```shell
    dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
    ```