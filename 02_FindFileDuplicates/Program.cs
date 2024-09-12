namespace _02_FindFileDuplicates
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
                    //                    Console.WriteLine($"Absolute path: {FilePathHelperLogic.GetAbsolutePath(input)}");
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }


            //StartMethods();
        }

        //public static void StartMethods()
        //{
        //    TestClass testClass = new TestClass();

        //    Task[] tasks = [testClass.CallLongMethodOne(10, 500),
        //                    testClass.CallLongMethodTwo(20, 400)];

        //    while (tasks.Any(t => !t.IsCompleted))
        //    {
        //        Console.WriteLine($"Method one: {(testClass.PercentLongMethodOne * 100):f2} %; Method two: {(testClass.PercentLongMethodTwo * 100):f2} %; ");
        //        //Task.Delay(1000);
        //        Thread.Sleep(1000);
        //    }

        //    // Now, we await them all.
        //    Task.WaitAll(tasks);
        //    //            await testClass.CallLongMethodOne(10, 100);
        //    //            await testClass.CallLongMethodTwo(10, 100);
        //}
    }
}
