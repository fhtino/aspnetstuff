using SharedObjects;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Security.Policy;
using System.Threading;
using System.Xml.Serialization;


namespace SoapLikeClient
{
    public class Program
    {

        //private const string URL = "http://localhost:65080/api.ashx";
        private const string URL = "https://localhost:44340/api.ashx";


        static void Main(string[] args)
        {
            StressAPI(); return;

            var client = new SimpleWrapper(URL);

            var authResp = client.Authenticate(new AuthRequest() { UserName = "foo", Password = "bar" });
            client.SetAuthToken(authResp.Token);
            Console.WriteLine($"AuthToken:  {authResp.Token}");

            var response = client.GetWeather(new GetWeatherRequest { City = "New York" });
            Console.WriteLine($"GetWeather: {response.Weather} - Temperature: {response.Temperature} - Messages: {string.Join(" + ", response.Messages)}");

            var getBigDataResp = client.GetBigData(new GetBigDataRequest() { DataSize = 10 * 1024 * 1024 });
            Console.WriteLine($"GetBigData: Length: {getBigDataResp.Data.Length}");

            var setBigDataResp = client.SetBigData(new SetBigData() { Data = new byte[7 * 1024 * 1024] });
            Console.WriteLine($"SetBigData: Size: {setBigDataResp.ReceivedDataSize} HttpRequestSize: {setBigDataResp.RequestSize}\n");
        }


        private static void StressAPI()
        {
            int numOfThreads = 40;

            var threads = new Thread[numOfThreads];

            for (int i = 0; i < numOfThreads; i++)
            {
                threads[i] = new Thread(
                    () =>
                    {
                        var sw = Stopwatch.StartNew();
                        var client = new SimpleWrapper(URL);
                        Console.WriteLine($"Calling...");
                        var resp = client.FakeCalculate(new FakeCalculateRequest() { Time = 5, Load = true });
                        Console.WriteLine($"Caller={sw.Elapsed.TotalSeconds.ToString("0.000")}  API={resp.ElapsedTime.ToString("0.000")}  DataCounter={resp.DataCounter}");
                    });
                threads[i].IsBackground = true;
                threads[i].Start();
            }

            for (int i = 0; i < numOfThreads; i++) threads[i].Join();
        }

    }
}
