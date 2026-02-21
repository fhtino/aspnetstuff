using SharedObjects;
using System;
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

        static void Main(string[] args)
        {
            //string url = "http://localhost:65080/api.ashx";
            string url = "https://localhost:44340/api.ashx";

            var client = new SimpleWrapper(url);

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

    }
}
