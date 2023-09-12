using System.Collections.Generic;

List<string> names = new List<string>();
var input = "";
do
{
    Console.WriteLine("\nType a name or type 'e' to exit");
    input = Console.ReadLine();
    if (input != "e")
        names.Add(input);

} while (input.ToLower() != "e");


PrintNames(names);


void PrintNames(List<string> names)
{
    Console.WriteLine("\nNames Ascending:");
    foreach (string name in names)
    {
        Console.WriteLine(name);
    }

    Console.WriteLine("\nNames Descending:");
    names.Reverse();
    foreach (string name in names)
    {
        Console.WriteLine(name);
    }

    Console.WriteLine("\nNames and Remove:");
    while (names.Count > 0)
    {
        Console.WriteLine(names[0]);
        names.RemoveAt(0);
    }

    names.Clear();
    Console.WriteLine($"\nNames is empty? {(names.Count == 0)}");
}