using SharedObjects;
using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;


namespace SoapLikeClient
{

    public class SimpleWrapper
    {

        private string _url;
        private string _authToken;


        public SimpleWrapper(string url)
        {
            _url = url;
            _authToken = null;
        }


        public void SetAuthToken(string authToken)
        {
            _authToken = authToken;
        }


        public AuthResponse Authenticate(AuthRequest req) { return (AuthResponse)DoHtppCall("Authenticate", req, typeof(AuthResponse)); }

        public GetWeatherResponse GetWeather(GetWeatherRequest req) { return (GetWeatherResponse)DoHtppCall("GetWeather", req, typeof(GetWeatherResponse)); }

        public GetBigDataResponse GetBigData(GetBigDataRequest req) { return (GetBigDataResponse)DoHtppCall("GetBigData", req, typeof(GetBigDataResponse)); }

        public SetBigDataResponse SetBigData(SetBigData req) { return (SetBigDataResponse)DoHtppCall("SetBigData", req, typeof(SetBigDataResponse)); }


        // -----------------------
        // --- Private methods ---
        // -----------------------


        private object DoHtppCall(string actionName, object requestObject, Type responseType)
        {
            // Note: here we use the HttpWebRequest just for testing on "traditional/old" NET framework.
            //       We can also use any Http client library, like HttpClient.

            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(_url);

            httpRequest.Method = "POST";
            httpRequest.Headers["SL-Authorization"] = _authToken;
            httpRequest.Headers["SL-ActionName"] = actionName;

            using (var requestStream = httpRequest.GetRequestStream())
            {
                SerializeToStream(requestStream, requestObject);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            using (var responseStream = httpResponse.GetResponseStream())
            {
                return DeserializeFromStream(responseStream, responseType);
            }
        }


        private static object DeserializeFromStream(Stream inputStream, Type type)
        {
            var serializer = new XmlSerializer(type);
            var obj = serializer.Deserialize(inputStream);
            return obj;
        }


        private static void SerializeToStream(Stream outputStream, object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            var xsn = new XmlSerializerNamespaces();
            xsn.Add(string.Empty, string.Empty);
            serializer.Serialize(outputStream, obj, xsn);
        }

    }
}
