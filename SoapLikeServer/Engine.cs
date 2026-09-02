using SharedObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace SoapLikeServer
{
    public class Engine
    {

        private const long MaxRequestBodyBytes = 10 * 1024 * 1024; // 10 MB, keep aligned with MaxCharactersInDocument


        public async Task ProcessRequestAsync(HttpContext context)
        {
            var startDT = DateTime.UtcNow;

            var timeoutToken = context.Request.TimedOutToken;  // this values should come from web.config/system.web/httpRuntime/@executionTimeout but sometimes it is not honored by IIS (unknown reason)
                                                               // Alternative: declare explicit cancellation token:
                                                               //     var timeoutToken = new CancellationTokenSource(10 * 1000).Token;  

            // Read auth and action headers
            string authData = context.Request.Headers["SL-Authorization"];
            string actionName = context.Request.Headers["SL-ActionName"];
            SimpleLog.WriteLine($"ActionName: {actionName}");

            // Fast rejection when Content-Length is too large.
            if (context.Request.ContentLength > MaxRequestBodyBytes)
            {
                context.Response.StatusCode = 413; // Payload Too Large
                context.Response.Write("Request body too large.\n\n");
                return;
            }

            // if no action name --> "hello world" message
            if (String.IsNullOrEmpty(actionName))
            {
                context.Response.StatusCode = 200;
                context.Response.Write($"Hello world - {DateTime.UtcNow.ToString("O")}");
                return;
            }            

            try
            {
                // !!! Reflection here !!!
                // We assume the method takes a single input parameter and return a Task<T> where T is the response type

                var methodOnLogic = typeof(APILogic).GetMethod(actionName);
                if (methodOnLogic == null)
                {
                    context.Response.StatusCode = 404; // Not Found
                    context.Response.Write("Action not found.\n\n");
                    return;
                }

                bool apiAttributePresent = methodOnLogic.GetCustomAttributes(typeof(APIAttribute), false).FirstOrDefault() is APIAttribute aaaa;
                if (!apiAttributePresent)
                {
                    context.Response.StatusCode = 404; // Not Found
                    context.Response.Write("Action API not found.\n\n");
                    return;
                }

                var inputParameterType = methodOnLogic.GetParameters().FirstOrDefault()?.ParameterType;
                var invocationTaskResultProperty = methodOnLogic.ReturnType.GetProperty("Result");   // This is the property .Result of Task<T> returned by method on Logic class

                var logicInstance = new APILogic(authData);
                var trackingID = Guid.NewGuid().ToString();

                var requestObject = DeserializeFromStream(context.Request.InputStream, inputParameterType);
                var invocationTask = methodOnLogic.Invoke(logicInstance, new object[] { requestObject, timeoutToken });   // <<<== Invoke method!
                await (Task)invocationTask;

                var responseObject = invocationTaskResultProperty.GetValue(invocationTask) as SLActionBaseResponse;
                responseObject.StartDT = startDT;
                responseObject.EndDT = DateTime.UtcNow;
                responseObject.TrackingID = trackingID;
                responseObject.RequestSize = context.Request.ContentLength;

                context.Response.StatusCode = 200;
                SerializeToStream(context.Response.OutputStream, responseObject);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500; // Internal Server Error
                context.Response.Write("Error processing the request.");
                SimpleLog.WriteLine(ex.ToString());
                SimpleLog.WriteLine("Elapsed time: " + (DateTime.UtcNow - startDT).TotalSeconds.ToString() + " s");
            }
        }


        private object DeserializeFromStream(Stream inputStream, Type type)
        {
            // To prevent XXE attacks, we disable DTD processing and set XmlResolver to null.
            // We also set limits on the size of the XML document and the number of characters from entities.
            var settings = new System.Xml.XmlReaderSettings
            {
                DtdProcessing = System.Xml.DtdProcessing.Prohibit,
                XmlResolver = null,
                MaxCharactersFromEntities = 1024,
                MaxCharactersInDocument = MaxRequestBodyBytes
            };

            using (var reader = System.Xml.XmlReader.Create(inputStream, settings))
            {
                var serializer = new XmlSerializer(type);
                return serializer.Deserialize(reader);
            }
        }


        private void SerializeToStream(Stream outputStream, object obj)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            var xsn = new XmlSerializerNamespaces();
            xsn.Add(string.Empty, string.Empty);
            serializer.Serialize(outputStream, obj, xsn);
        }

    }
}