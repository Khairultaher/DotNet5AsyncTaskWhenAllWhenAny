using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet5AsyncTaskWhenAllWhenAny
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World from .NET5 !!");

            // Running synchronously – the slow way
            Console.WriteLine("# Running synchronously – the slow way");
            await RunningSynchronouslyTheSlowWay();

            //Running asynchronously – the faster way
            Console.WriteLine("# Running asynchronously – the faster way using when any");
            await RunningAsynchronouslyTheFasterWayWhenAnyAsync();

            //Running asynchronously – the faster way
            Console.WriteLine("# Running asynchronously – the faster way using when all");
            await RunningAsynchronouslyTheFasterWayWhenAllAsync();

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
        private static async Task RunningAsynchronouslyTheFasterWayWhenAnyAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // this task will take about 2.5s to complete
            var sumTask = SlowAndComplexSumAsync();

            // this task will take about 4s to complete
            var wordTask = SlowAndComplexWordAsync();

            // running them in parallel should take about 4s to complete

            List<Task> tasks = new List<Task> { sumTask, wordTask };
            while (tasks.Count > 0)
            {
                Task task = await Task.WhenAny(tasks);
                if (task == sumTask)
                {
                    Console.WriteLine("Result of complex sum = " + sumTask.Result);
                }
                else if (task == wordTask)
                {
                    Console.WriteLine("Result of complex letter processing " + wordTask.Result);
                }
                tasks.Remove(task);
            }
            
            // The elapsed time at this point will only be about 4s
            Console.WriteLine("Time elapsed when both complete..." + stopwatch.Elapsed);
        }
        private static async Task RunningAsynchronouslyTheFasterWayWhenAllAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // this task will take about 2.5s to complete
            var sumTask = SlowAndComplexSumAsync();

            // this task will take about 4s to complete
            var wordTask = SlowAndComplexWordAsync();

            // running them in parallel should take about 4s to complete
            await Task.WhenAll(sumTask, wordTask);

            // The elapsed time at this point will only be about 4s
            Console.WriteLine("Time elapsed when both complete..." + stopwatch.Elapsed);

            Console.WriteLine("Result of complex sum = " + sumTask.Result);
            Console.WriteLine("Result of complex letter processing " + wordTask.Result);
        }
        private static async Task RunningSynchronouslyTheSlowWay()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // This method takes about 2.5s to run
            var complexSum = await SlowAndComplexSumAsync();

            // The elapsed time will be approximately 2.5s so far
            Console.WriteLine("Time elapsed when sum completes..." + stopwatch.Elapsed);

            // This method takes about 4s to run
            var complexWord = await SlowAndComplexWordAsync();

            // The elapsed time at this point will be about 6.5s
            Console.WriteLine("Time elapsed when both complete..." + stopwatch.Elapsed);

            Console.WriteLine("Result of complex sum = " + complexSum);
            Console.WriteLine("Result of complex letter processing " + complexWord);
        }
        private static async Task<int> SlowAndComplexSumAsync()
        {
            int sum = 0;
            foreach (var counter in Enumerable.Range(0, 25))
            {
                sum += counter;
                await Task.Delay(100);
            }

            return sum;
        }
        private static async Task<string> SlowAndComplexWordAsync()
        {
            var word = string.Empty;
            foreach (var counter in Enumerable.Range(65, 26))
            {
                word = string.Concat(word, (char)counter);
                await Task.Delay(150);
            }

            return word;
        }
    }
}
