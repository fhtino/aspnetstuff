using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


namespace SoapLikeServer
{
    public class DirectAPI1 : HttpTaskAsyncHandler
    {

        // Not required. Already set to false inside HttpTaskAsyncHandler class.
        // public override bool IsReusable => false;

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            //var cts = new CancellationTokenSource(10 * 1000);
            //var token = cts.Token;

            await new Engine().ProcessRequestAsync(context);
        }

    }
}