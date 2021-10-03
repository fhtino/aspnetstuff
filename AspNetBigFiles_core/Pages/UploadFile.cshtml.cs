using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Pages
{
    public class UploadFileModel : PageModel
    {

        [BindProperty]
        public string UploadGUID { get; set; }


        public void OnGet()
        {
            UploadGUID = Guid.NewGuid().ToString();
        }


        public void OnPost()
        {
            // execute some operation on the file (e.g. move from temp to final storage)
            string fileName = Path.Combine(@"C:\temp\upload", UploadGUID);
            System.IO.File.Move(fileName, fileName + ".OK");

            // move to another page or other action
            Response.Redirect("/Index?uploadguid=" + UploadGUID);
        }


    }
}
