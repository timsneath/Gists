using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Extensions.Gists.Interop
{
    public class GistClient : System.Net.Http.HttpClient
    {
        public GistClient() : base()
        {
            this.BaseAddress = new Uri("https://api.github.com/");
            this.DefaultRequestHeaders.Accept.Clear();
            this.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            this.DefaultRequestHeaders.UserAgent.ParseAdd("GistForVisualStudio/1.0");
        }
    }
}
