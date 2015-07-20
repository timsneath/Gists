using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Extensions.Gists.Interop
{
    public class GistHttpClient : System.Net.Http.HttpClient
    {
        public GistHttpClient() : base()
        {
            this.BaseAddress = new Uri("https://api.github.com/");
            this.DefaultRequestHeaders.Accept.Clear();
            this.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            this.DefaultRequestHeaders.UserAgent.ParseAdd("GistForVisualStudio/1.0");
        }

        public GistHttpClient(string token) : this()
        {
            this.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("token", token);
        }
    }
}
