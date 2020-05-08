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
            var githubMDProxy = new GitHubMDProxy(mdUrl);
            ContentDiv.InnerHtml = githubMDProxy.GetHtml(null).Result;
        }
    }
}