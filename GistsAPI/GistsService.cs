using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.Extensions.Gists.Interop
{
    public class GistsService : IDisposable
    {
        private GistHttpClient httpClient;
        public GistsService()
        {
            this.httpClient = new GistHttpClient();
        }

        public GistsService(string oAuthToken)
        {
            this.httpClient = new GistHttpClient(oAuthToken);
        }

        public async Task<JArray> GetGistsAsync(string username)
        {
            string relativeUri;
            if (username == "public")
            {
                relativeUri = "/gists/public";
            }
            else
            {
                relativeUri = "/users/" + username + "/gists";
            }

            var responseMessage = await httpClient.GetAsync(relativeUri);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            JArray response = JArray.Parse(responseContent);
            return response;
        }

        public async Task<JArray> GetGistsAsync()
        {
            return await GetGistsAsync("public");
        }

        public async Task<JObject> PostNewGistAsync(string codeSnippet, string description, string filename, bool isPublic = true)
        {
            JObject json = new JObject {
                { "description", description },
                { "public", isPublic },
                { "files", new JObject {
                    { "filename", new JObject {
                        { "content", codeSnippet } } } } } };

            var content = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("/gists", content);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            return JObject.Parse(responseContent);
        }

        public void Dispose()
        {
            ((IDisposable)this.httpClient).Dispose();
        }
    }
}
