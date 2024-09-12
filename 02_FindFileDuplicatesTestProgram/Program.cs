using _02_FindFileDuplicates;

namespace _02_FindFileDuplicatesTestProgram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test find file duplicates with progress information!");

            while (true)
            {
                Console.Write("Enter a path to for duplicates - 'q' to quit: ");
                string? input = Console.ReadLine();

                if (input == null)
                    continue;

                if (input == "q")
                    break;

                try
                {
                    FindFileDuplicatesLogic findFileDuplicatesLogic = new();

                    Task<IEnumerable<string>> taskCompile = Task.Run(() => (IEnumerable<string>)findFileDuplicatesLogic.CompileCandidates(input));

                    while (!taskCompile.IsCompleted)
                    {
                        Console.Write($"\rCompile candidates: {findFileDuplicatesLogic.PercentageCompileCandidates:f2}%");
                        Thread.Sleep(500);
                    }

                    Console.Write($"\r{100}%                                ");
                    Console.WriteLine();
                    taskCompile.Wait();
                    Console.WriteLine("candidates:");
                    foreach (string path in taskCompile.Result)
                        Console.WriteLine($"{path}");

                    Task<IEnumerable<string>> taskCheck = Task.Run(() => (IEnumerable<string>)findFileDuplicatesLogic.CheckCandidates(taskCompile.Result));

                    while (!taskCheck.IsCompleted)
                    {
                        Console.Write($"\rCheck candidates: {findFileDuplicatesLogic.PercentageCheckCandidates:f2}%");
                        Thread.Sleep(500);
                    }

                    Console.Write($"\r{100}%                            ");
                    Console.WriteLine();
                    taskCheck.Wait();
                    Console.WriteLine("duplicates:");
                    foreach (string path in taskCheck.Result)
                        Console.WriteLine($"{path}");
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }
        }
    }
}