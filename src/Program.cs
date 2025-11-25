namespace MultiAgentLearning;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("================================================================");
        Console.WriteLine("     Multi-Agent Learning System - Console (Deprecated)        ");
        Console.WriteLine("================================================================");
        Console.WriteLine();
        Console.WriteLine("NOTE: The console version has been replaced with a WPF UI.");
        Console.WriteLine();
        Console.WriteLine("To run the application:");
        Console.WriteLine("   1. In Visual Studio Solution Explorer:");
        Console.WriteLine("      - Right-click on 'MultiAgentLearning.UI' project");
        Console.WriteLine("      - Select 'Set as Startup Project'");
        Console.WriteLine("      - Press F5 to run");
        Console.WriteLine();
        Console.WriteLine("   2. Or from command line:");
        Console.WriteLine("      cd src\\MultiAgentLearning.UI");
        Console.WriteLine("      dotnet run");
        Console.WriteLine();
        Console.WriteLine("The UI provides:");
        Console.WriteLine("   * Interactive question input");
        Console.WriteLine("   * Real-time agent status tracking");
        Console.WriteLine("   * Sequential and Concurrent orchestration modes");
        Console.WriteLine("   * Result copying and management");
        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
