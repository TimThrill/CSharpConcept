using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSharpConcept
{
    class Program
    {
        private static HttpClient _httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            var watch = Stopwatch.StartNew();
            var repeats = 1000;

            #region CPU intensive job
            Console.WriteLine("Start CPU intensive job");
            RepeatingJobRunner(repeats, Divide);
            watch.Stop();
            Console.WriteLine("Synchronous takes {0}s.", watch.Elapsed.TotalSeconds);

            // Asynchronous with separate threads
            watch.Restart();

            await RepeatingJobRunnerAsync(repeats, Divide, true);
            watch.Stop();
            Console.WriteLine("Asynchronous with separate threads takes {0}s.", watch.Elapsed.TotalSeconds);

            // Asynchronous without separate threads
            watch.Restart();

            await RepeatingJobRunnerAsync(repeats, Divide, false);
            watch.Stop();
            Console.WriteLine("Asynchronous without separate threads takes {0}s.", watch.Elapsed.TotalSeconds);
            Console.WriteLine("End");
            Console.WriteLine();
            #endregion

            #region Network IO intensive job

            Console.WriteLine("Start Network IO intensive job");
            repeats = 100;
            watch.Restart();
            RepeatingJobRunner(repeats, GetWebPage);
            watch.Stop();
            Console.WriteLine("Synchronous takes {0}s.", watch.Elapsed.TotalSeconds);

            // Asynchronous without separate threads
            watch.Restart();

            await RepeatingJobRunnerAsync(repeats, GetWebPage, false);
            watch.Stop();
            Console.WriteLine("Asynchronous without separate threads takes {0}s.", watch.Elapsed.TotalSeconds);

            // Asynchronous with separate threads
            watch.Restart();

            await RepeatingJobRunnerAsync(repeats, GetWebPage, true);
            watch.Stop();

            Console.WriteLine("Asynchronous with separate threads takes {0}s.", watch.Elapsed.TotalSeconds);
            Console.WriteLine("End");
            #endregion
        }

        private static void RepeatingJobRunner(int repeatTimes, Func<Task> func)
        {
            for (int i = 0; i < repeatTimes; i++)
            {
                func().Wait();
            }
        }

        private static async Task RepeatingJobRunnerAsync(int repeatTimes, Func<Task> func, bool useSeparateThread)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < repeatTimes; i++)
            {
                if (useSeparateThread)
                {
                    tasks.Add(Task.Run(() => func()));
                }
                else
                {
                    tasks.Add(func());
                }
            }

            await Task.WhenAll(tasks);
        }

        private static Task Divide()
        {
            var a = 10;
            var b = 3.0m;

            for (var i = 0; i < 10000; i++)
            {
                decimal.Parse((a / b).ToString());
            }

            return Task.CompletedTask;
        }

        private static Task GetWebPage()
        { 
            return _httpClient.GetAsync("https://www.google.com");
        }
    }
}
