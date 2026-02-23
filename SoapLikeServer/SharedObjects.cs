using System;
using System.Xml.Serialization;

namespace SharedObjects
{

    public class SLActionBaseRequest
    {
    }


    public class SLActionBaseResponse
    {
        [XmlAttribute]
        public int ErrorCode { get; set; }

        [XmlAttribute]
        public string TrackingID { get; set; }

        [XmlAttribute]
        public DateTime StartDT { get; set; }

        [XmlAttribute]
        public DateTime EndDT { get; set; }

        [XmlAttribute]
        public int RequestSize { get; set; }
    }


    public class AuthRequest : SLActionBaseRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponse : SLActionBaseResponse
    {
        public string Token { get; set; }
    }


    public class GetWeatherRequest : SLActionBaseRequest
    {
        public string City { get; set; }
    }


    public class GetWeatherResponse : SLActionBaseResponse
    {
        public string Weather { get; set; }

        public int Temperature { get; set; }

        public string[] Messages { get; set; }

    }


    public class GetBigDataRequest : SLActionBaseRequest
    {
        public int DataSize { get; set; }
    }


    public class GetBigDataResponse : SLActionBaseResponse
    {
        public byte[] Data { get; set; }
    }


    public class SetBigData : SLActionBaseRequest
    {
        public byte[] Data { get; set; }
    }


    public class SetBigDataResponse : SLActionBaseResponse
    {
        public int ReceivedDataSize { get; set; }
    }


    public class FakeCalculateRequest : SLActionBaseRequest
    {
        public int Time { get; set; }
        public bool Load { get; set; }
    }

    public class FakeCalculateResponse : SLActionBaseResponse
    {
        public double ElapsedTime { get; set; }
        public long DataCounter { get; set; }
    }

}