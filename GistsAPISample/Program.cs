using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Extensions.Gists.Interop;

namespace Microsoft.VisualStudio.Extensions.Gists
{
    class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                await PostGist();
                await GetGists();

            }).Wait();
            Console.ReadLine();
        }

        private static async Task PostGist()
        {
            Console.WriteLine("Post Gist Test");
            using (var service = new GistsService())
            {
                var uri = await service.PostNewGistAsync("here is a sample code snippet " + Guid.NewGuid().ToString(), "description " + Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), true);
                Console.WriteLine("Gist posted at " + uri.ToString());
                Console.WriteLine();
            }
        }

        private static async Task GetGists()
        {
            Console.WriteLine("Get Gists Test");

            using (var service = new GistsService())
            {
                var gists = await service.GetGistsAsync("timsneath");

                Console.WriteLine("There were " + gists.Count + " entries returned.");
                Console.WriteLine();
                Console.WriteLine("GistsService response: ");

                foreach (var gist in gists)
                {
                    Console.WriteLine(gist["description"]);
                }
            }
        }
    }
}
