using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace AsyncClient
{

    internal class Program
    {

        static async Task<int> Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 1000;

            int k = 200;
            var tList = new List<Thread>();
            int running = 0;

            for (int i = 0; i < k; i++)
            {
                await Task.Delay(new Random().Next(100));

                var t = new Thread(
                            async () =>
                            {
                                var httpClient = new HttpClient();
                                while (true)
                                {
                                    Interlocked.Increment(ref running);
                                    var startDT = DateTime.UtcNow;
                                    string s = await httpClient.GetStringAsync("http://localhost:55948/HandlerSync.ashx");
                                    Interlocked.Decrement(ref running);
                                    Console.WriteLine($"{running} : {DateTime.UtcNow.Subtract(startDT).TotalSeconds.ToString("0.000")} : {s}");                                    
                                    //Thread.Sleep(1000);
                                }
                            });
                t.IsBackground = true;
                t.Start();
                Thread.Sleep(50);
            }

            Console.WriteLine(new string('*', 60));
            Console.WriteLine(new string('*', 60));
            Console.WriteLine(new string('*', 60));
            Console.WriteLine("Press a key to exit");
            Console.ReadKey();

            return 0;
        }
    }

}
