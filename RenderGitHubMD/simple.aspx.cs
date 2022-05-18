using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RenderGitHubMD
{
    public partial class simple : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string mdUrl = "https://github.com/fhtino/aspnetstuff/blob/master/RenderGitHubMD/mdsample/sample1.md";

            string imgid = Request["imgid"];

            var githubMDProxy = new GitHubMDProxy(mdUrl);

            if (imgid == null)
            {
                ContentDiv.InnerHtml = githubMDProxy.GetHtml("?imgid=").Result;
            }
            else
            {
                Tuple<byte[], string> imageData = githubMDProxy.GetImage(imgid).Result;
                byte[] imgBody = imageData.Item1;
                string contentType = imageData.Item2;

                Response.Clear();
                Response.ContentType = contentType;
                Response.OutputStream.Write(imgBody, 0, imgBody.Length);
                Response.End();
            }

        }
    }
}