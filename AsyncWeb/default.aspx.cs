using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AsyncWeb
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int availWorkerThreads;
            int availCompletionPortThreads;
            int maxWorkerThreads;
            int maxCompletionPortThreads;
            int minWorkerThreads;
            int minCompletionPortThreads;

            ThreadPool.GetAvailableThreads(out availWorkerThreads, out availCompletionPortThreads);
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
            ThreadPool.GetMinThreads(out minWorkerThreads, out minCompletionPortThreads);

            var msg = new List<string>();
            msg.Add($"Avail: {availWorkerThreads} {availCompletionPortThreads}");
            msg.Add($"Max__: {maxWorkerThreads} {maxCompletionPortThreads}");
            msg.Add($"Min__: {minWorkerThreads} {minCompletionPortThreads}");
            msg.Add($"Used_: {maxWorkerThreads - availWorkerThreads} {maxCompletionPortThreads - availCompletionPortThreads}");

            LblMsg.Text = String.Join("<br/>", msg);
        }
    }
}