using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RenderGitHubMD
{
    public partial class full : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string imgid = Request["imgid"];
            string mdUrl = "https://github.com/fhtino/aspnetstuff/blob/master/RenderGitHubMD/mdsample/sample1.md";

            var githubMDProxy = new GitHubMDProxy(mdUrl);

            if (imgid == null)
            {
                string cacheKey = "my-html-" + mdUrl;
                string htmlFragment = Cache[cacheKey] as string;
                if (htmlFragment == null)
                {
                    htmlFragment = githubMDProxy.GetHtml("full.aspx?imgid=").Result;
                    Cache.Insert(cacheKey, htmlFragment, null, DateTime.UtcNow.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                ContentDiv.InnerHtml = htmlFragment;
                LinkToSource.NavigateUrl = mdUrl;
                LinkToSource.Text = mdUrl;
            }
            else
            {
                string cacheKeyIMG = "my-img_" + imgid;
                string cacheKeyCT = "my-img_" + imgid + "_ct";
                byte[] imgBody = Cache[cacheKeyIMG] as byte[];
                string contentType = Cache[cacheKeyCT] as string;

                if (imgBody == null || contentType == null)
                {
                    Tuple<byte[], string> imageData = githubMDProxy.GetImage(imgid).Result;
                    imgBody = imageData.Item1;
                    contentType = imageData.Item2;
                    Cache.Insert(cacheKeyIMG, imgBody, null, DateTime.UtcNow.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
                    Cache.Insert(cacheKeyCT, contentType, null, DateTime.UtcNow.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
                }

                Response.Clear();
                Response.ContentType = contentType;
                Response.OutputStream.Write(imgBody, 0, imgBody.Length);
                Response.End();
            }
        }
    }
}