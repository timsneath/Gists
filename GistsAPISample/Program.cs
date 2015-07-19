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
                var service = new GistsService();
                var gists = await service.GetGistsAsync();

                //Console.WriteLine("There were " + gists.Count + " entries returned.");
                //Console.WriteLine();
                Console.WriteLine("GistsService response: ");

                Console.WriteLine(gists.ToString());
            }).Wait();
        }   
    }
}
