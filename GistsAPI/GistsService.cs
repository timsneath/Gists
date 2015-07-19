using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.Extensions.Gists.Interop
{
    public class GistsService
    {
        public async Task<string> GetGistsAsync(string username)
        {
            using (var client = new GistClient())
            {
                string relativeUri;
                if (username == "public")
                {
                    relativeUri = "/gists/public";
                }
                else
                {
                    relativeUri = "/users/:" + username + "/gists";
                }

                var responseMessage = await client.GetAsync(relativeUri);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                return responseContent;
                //JObject response = JObject.Parse(await responseMessage.Content.ReadAsStringAsync());
                //return response.ToString();
            }
        }

        public async Task<string> GetGistsAsync()
        {
            return await GetGistsAsync("public");
        }

        public async Task<JObject> PostNewGistAsync(string codeSnippet, string description, string filename, bool isPublic = true)
        {
            using (var client = new GistClient())
            {
                JObject json = new JObject
                {
                    { "description", description },
                    { "public", isPublic },
                    { "files", new JObject
                        {
                            filename, new JObject
                            {
                                "content", codeSnippet
                            }
                        }
                    }
                };
                //string json = "{ \n" +
                //                 "\"description\": \"" + description + "\",\n" +
                //                 "\"public\": " + isPublic.ToString().ToLower() + ",\n" +
                //                 "\"files\": {\n" +
                //                    "\"" + filename + "\": {" + "\n" +
                //                       "\"content\": \"" + codeSnippet + "\" " + "\n" +
                //                    "}" + "\n" +
                //                 "}" + "\n" +
                //              "}";

                StringContent content = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");

                var responseMessage = await client.PostAsync("/gists", content);

                JObject response = JObject.Parse(await responseMessage.Content.ReadAsStringAsync());
                return response;
            }
        }
    }
}
