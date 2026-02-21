using SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Web;

namespace SoapLikeServer
{



    public class Logic
    {

        private string _authToken;

        public Logic(string authToken)
        {
            _authToken = authToken;
        }


        [API]
        public async Task<AuthResponse> Authenticate(AuthRequest request)
        {
            await Task.CompletedTask;
            await Task.Delay(100); // Simulate some delay

            // if....

            return new AuthResponse
            {
                ErrorCode = 0,
                Token = "fake_token_0123456789012345678901234567890123456789"
            };
        }


        [API]
        public async Task<GetWeatherResponse> GetWeather(GetWeatherRequest request)
        {
            if (!ValidateAuthToken()) return new GetWeatherResponse() { ErrorCode = 999 };

            await Task.CompletedTask;
            await Task.Delay(200); // Simulate some delay

            return new GetWeatherResponse
            {
                ErrorCode = -1,
                Weather = $"Sunny in {request.City} ",
                Temperature = 25,
                Messages = new string[] { "Have a nice day!", "Don't forget to drink water!" }
            };
        }


        [API]
        public async Task<GetBigDataResponse> GetBigData(GetBigDataRequest request)
        {
            if (!ValidateAuthToken()) return new GetBigDataResponse() { ErrorCode = 999 };
            await Task.CompletedTask;

            if (request.DataSize > 100 * 1024 * 1024) // more than 100 MB
            {
                return new GetBigDataResponse
                {
                    ErrorCode = 123,
                    Data = null
                };
            }

            var buffer = new byte[request.DataSize];
            new Random().NextBytes(buffer);
            return new GetBigDataResponse
            {
                ErrorCode = -1,
                Data = buffer
            };
        }


        [API]
        public async Task<SetBigDataResponse> SetBigData(SetBigData request)
        {
            if (!ValidateAuthToken()) return new SetBigDataResponse() { ErrorCode = 999 };

            await Task.CompletedTask;           

            return new SetBigDataResponse
            {
                ErrorCode = -1,
                ReceivedDataSize = request.Data?.Length ?? 0
            };
        }


        private bool ValidateAuthToken()
        {
            return !String.IsNullOrEmpty(_authToken);   // everything is fine, except empty or null tokens :)
        }

    }

}