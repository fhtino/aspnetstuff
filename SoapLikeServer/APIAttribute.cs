using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoapLikeServer
{

    public class APIAttribute : Attribute
    {
        public string Name { get; set; }
    }

}