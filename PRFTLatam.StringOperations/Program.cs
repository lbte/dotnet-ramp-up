using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
public class Program
{
    public static void Main()
    {
        StringOperations stringsIntance = new StringOperations(); 
        stringsIntance.ConcatenateNames();
        // var config = new ManualConfig()
        // .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        // .AddValidator(JitOptimizationsValidator.DontFailOnError)
        // .AddLogger(ConsoleLogger.Default)
        // .AddColumnProvider(DefaultColumnProviders.Instance);

        var summary = BenchmarkRunner.Run<StringOperations>();
    }
}

[MemoryDiagnoser, RankColumn]
public class StringOperations
{
    public string name1 = "Tonny";
    public string name2 = "Anderson";
    public string name3 = "Stark";
    public string name4 = "Rogers";

    // public void ConcatenateNames(string name1, string name2, string name3, string name4)
    public void ConcatenateNames()
    {
        Console.WriteLine("\nPlus (+) Operator:");
        Console.WriteLine(ConcatenatePlusOperator());

        Console.WriteLine("\nPlus Equal (+=) Operator:");
        Console.WriteLine(ConcatenatePlusEqualOperator());
        
        Console.WriteLine("\nString Interpolation:");
        Console.WriteLine(ConcatenateStringInterpolation());

        Console.WriteLine("\nString Format:");
        Console.WriteLine(ConcatenateStringFormat());

        Console.WriteLine("\nString Builder:");
        Console.WriteLine(ConcatenateStringBuilder());

        Console.WriteLine("\nString Concat:");
        Console.WriteLine(ConcatenateStringConcat());

        Console.WriteLine("\nString Join:");
        Console.WriteLine(ConcatenateStringJoin());

        Console.WriteLine("\nLINQ and Enumerable.Aggregate:");
        Console.WriteLine(ConcatenateLINQ());

    }

    [Benchmark]
    public string ConcatenatePlusOperator()
    {
        return "Hello " + name1 + " " + name2 + " " + name3 + " " + name4; 
    }

    [Benchmark]
    public string ConcatenatePlusEqualOperator()
    {
        string names = "Hello ";
        names += name1;
        names += name2;
        names += name3;
        names += name4;
        return names; 
    }

    [Benchmark]
    public string ConcatenateStringInterpolation()
    {
        return $"Hello {name1} {name2} {name3} {name4}"; 
    }

    [Benchmark]
    public string ConcatenateStringFormat()
    {
        return string.Format("Hello {0} {1} {2} {3}", name1, name2, name3, name4); 
    }

    [Benchmark]
    public string ConcatenateStringBuilder()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("Hello");
        sb.Append(name1 + " ");
        sb.Append(name2 + " ");
        sb.Append(name3 + " ");
        sb.Append(name4);
        return sb.ToString(); 
    }

    [Benchmark]
    public string ConcatenateStringConcat()
    {
        return string.Concat("Hello", name1, name2, name3, name4);
    }

    [Benchmark]
    public string ConcatenateStringJoin()
    {
        string[] names = {"Hello", name1, name2, name3, name4};
        return string.Join(" ", names);
    }

    [Benchmark]
    public string ConcatenateLINQ()
    {
        string[] names = {"Hello", name1, name2, name3, name4};
        // creates an intermediate string for each iteration.
        return names.Aggregate((partialNames, name) => $"{partialNames} {name}");
    }
}