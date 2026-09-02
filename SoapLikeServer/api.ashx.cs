using SharedObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;


namespace SoapLikeServer
{

    public class api : HttpTaskAsyncHandler
    {

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            await new Engine().ProcessRequestAsync(context);
        }

    }

}