using SharedObjects;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


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


        [API]
        public async Task<FakeCalculateResponse> FakeCalculate(FakeCalculateRequest request)
        {
            var sw = Stopwatch.StartNew();
            long dataCounter = 0;

            if (request.Time > 10) return new FakeCalculateResponse { ErrorCode = 123, ElapsedTime = 0 }; // too much time requested

            if (request.Load)
            {
                while (sw.Elapsed.TotalSeconds < request.Time)
                {
                    // 100% cpu-core load                  
                    dataCounter++;
                }
            }
            else
            {
                await Task.Delay(request.Time * 1000);
            }

            return new FakeCalculateResponse
            {
                ErrorCode = -1,
                ElapsedTime = sw.Elapsed.TotalSeconds,
                DataCounter = dataCounter
            };
        }


        private bool ValidateAuthToken()
        {
            return !String.IsNullOrEmpty(_authToken);   // everything is fine, except empty or null tokens :)
        }

    }

}