using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InjectValuesInWebConfig
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var lines = new List<string>();
            lines.Add("the K1 has value " + ConfigurationManager.AppSettings["K1"]);

            lines.Add("");
            lines.Add("AppSettings:");
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                lines.Add($" - {key}={ConfigurationManager.AppSettings[key]}");
            }

            lines.Add("");
            lines.Add("ConnectionStrings:");
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                lines.Add($" - {ConfigurationManager.ConnectionStrings[i].Name} = {ConfigurationManager.ConnectionStrings[i].ConnectionString}");
            }            

            LblOut.Text = String.Join("<br/>", lines);
        }
    }
}