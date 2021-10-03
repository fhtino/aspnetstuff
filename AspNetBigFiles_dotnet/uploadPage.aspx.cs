using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web1
{
    public partial class uploadPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.UploadGuid.Value = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss") + "_" + Guid.NewGuid().ToString().Replace("-", "");
            }
            else
            {
                var uploadguid = UploadGuid.Value;

                // execute some operation on the file (e.g. move from temp to final storage)
                string fileName = Path.Combine(@"C:\temp\upload", uploadguid);
                File.Move(fileName, fileName + ".OK");

                // move to another page or other action
                Response.Redirect("default.aspx?fyi=" + uploadguid);
            }
        }
    }
}