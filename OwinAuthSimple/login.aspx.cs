using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OwinAuthSimple
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<string> providerNames =
                    HttpContext.Current.GetOwinContext().Authentication
                        .GetAuthenticationTypes((d) => d.Properties != null && d.Properties.ContainsKey("Caption"))
                        .Select(t => t.AuthenticationType).ToList();

                ListView1.DataSource = providerNames;
                ListView1.DataBind();
            }
            else
            {
                var provider = Request.Form["provider"];
                if (provider == null)
                {
                    return;
                }
                else
                {
                    string returnUrl = Request["ReturnUrl"];

                    if (String.IsNullOrEmpty(returnUrl))
                        returnUrl = "/";

                    var properties = new Microsoft.Owin.Security.AuthenticationProperties
                    {
                        RedirectUri = returnUrl
                    };

                    HttpContext.Current.GetOwinContext().Authentication.Challenge(properties, provider);

                }
            }
        }
    }
}