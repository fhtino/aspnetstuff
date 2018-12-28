using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OwinAuthSimple
{
    public partial class main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var cIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (cIdentity != null && cIdentity.IsAuthenticated)
            {
                GVUserClaims.DataSource = cIdentity.Claims.Select(c => new { c.Type, c.Value });
                GVUserClaims.DataBind();
            }
        }
    }
}