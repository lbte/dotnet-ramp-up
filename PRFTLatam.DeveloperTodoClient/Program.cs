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