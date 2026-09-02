using SharedObjects;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Description;


namespace SoapLikeServer
{

    public class APILogic
    {

        private string _authToken;

        public APILogic(string authToken)
        {
            _authToken = authToken;
        }


        [API]
        public async Task<AuthResponse> Authenticate(AuthRequest request, CancellationToken ct)
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
        public async Task<GetWeatherResponse> GetWeather(GetWeatherRequest request, CancellationToken ct)
        {
            if (!ValidateAuthToken()) return new GetWeatherResponse() { ErrorCode = 999 };

            await Task.CompletedTask;
            await Task.Delay(200); // Simulate some delay

            return new GetWeatherResponse
            {
                ErrorCode = -1,
                Weather = $"Sunny in {request.City}.",
                Temperature = 25,
                Messages = new string[] { "Have a nice day!", "Stay safe!" }
            };
        }


        [API]
        public async Task<GetBigDataResponse> GetBigData(GetBigDataRequest request, CancellationToken ct)
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
        public async Task<SetBigDataResponse> SetBigData(SetBigData request, CancellationToken ct)
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
        public async Task<FakeCalculateResponse> FakeCalculate(FakeCalculateRequest request, CancellationToken ct)
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


        [API]
        public async Task<LongWaitResponse> LongWait(LongWaitRequest request, CancellationToken ct)
        {
            // experiments...

            var sw = Stopwatch.StartNew();

            if (false)
            {
                //await Task.Delay(request.Seconds * 1000, ct);
                await Task.Delay(request.Seconds * 1000);
                
            }

            if (true)
            {
                while (true)
                {
                    await Task.Delay(1000, ct);
                    if (sw.Elapsed.TotalSeconds >= request.Seconds) { break; }

                    string logFilePath = "c://temp//mylog.txt";
                    string logEntry = $"SERVER: {DateTime.UtcNow.ToString("O")} - {sw.Elapsed.TotalSeconds}\n";
                    File.AppendAllText(logFilePath, logEntry);
                }

            }

            return new LongWaitResponse
            {
                ErrorCode = -1,
                ElapsedTime = sw.Elapsed.TotalSeconds
            };
        }



        // ------------------------------------------------------------------------------------------------------------


        private bool ValidateAuthToken()
        {
            return !String.IsNullOrEmpty(_authToken);   // this is just a placeholder: everything is fine, except empty or null tokens :)
        }

    }

}