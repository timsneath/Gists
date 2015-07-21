using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Extensions.Gists.Interop;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Extensions.Gists.Tests
{
    [TestClass]
    public class TestGistsAPI
    {
        [TestMethod]
        public async Task TestPostNewGist()
        {
            using (var service = new GistsService())
            {
                var uri = await service.PostNewGistAsync("here is a sample code snippet " + Guid.NewGuid().ToString(), 
                    "description " + Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), true);
                Assert.IsTrue(uri.Host == "gist.github.com");
                Debug.WriteLine("Gist posted at " + uri.ToString());
            }
        }

        [TestMethod]
        public async Task TestGetGists()
        {
            using (var service = new GistsService())
            {
                var gists = await service.GetGistsAsync("timsneath");

                Assert.IsTrue(gists.Count > 0);
            }
        }

        [TestMethod]
        public async Task TestGetPublicGists()
        {
            using (var service = new GistsService())
            {
                var gists = await service.GetGistsAsync();

                Assert.IsTrue(gists.Count > 0);
            }
        }

    }
}
