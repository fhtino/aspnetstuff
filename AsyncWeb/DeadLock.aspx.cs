using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AsyncWeb
{
    public partial class DeadLock : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            MySleep().Wait();
        }

        private async Task MySleep()
        {
            // Deadlock, because here ".ConfigureAwait(false)" is missing and
            // the caller (Page_load) is not async. This combination rises a deadlock.
            //
            // To solve: await Task.Delay(100).ConfigureAwait(false);

            await Task.Delay(100);
        }
    }
}