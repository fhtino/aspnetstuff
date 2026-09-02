using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace SoapLikeServer
{
    public class DirectAPI2 : HttpTaskAsyncHandler, IRouteHandler
    {

        // Not required. Already set to false inside HttpTaskAsyncHandler class.
        // public override bool IsReusable => false;

        public IHttpHandler GetHttpHandler(RequestContext requestContext) { return this; }  // required by IRouteHandler interface

        
        public override async Task ProcessRequestAsync(HttpContext context)
        {
            await new Engine().ProcessRequestAsync(context);
        }

    }
}